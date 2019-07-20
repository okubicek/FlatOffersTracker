using FlatOffersTracker.Cqrs;
using FlatOffersTracker.DataAccess;
using FlatOffersTracker.Entities;
using FlatOffersTracker.Parsing;
using Serilog;
using System;
using System.Collections.Generic;

namespace FlatOffersTracker
{
	public class TractOffersCommandHandler : ICommand
	{
		private IExecutionHistoryRepository _executionHistoryRepository;

		private IFlatOffersRepository _flatOffersRepository;

		private ICommand<IEnumerable<FlatOffer>, UpdateOffersBasedOnAdvertisementsCommand> _updateOffers;

		private IEnumerable<IAdvertisementsCollector> _advertisementsCollectors;

		private ILogger _logger;

		public TractOffersCommandHandler(IExecutionHistoryRepository executionHistoryRepository, 
			IFlatOffersRepository flatOffersRepository,
			ICommand<IEnumerable<FlatOffer>, UpdateOffersBasedOnAdvertisementsCommand> updateOffers,
			IEnumerable<IAdvertisementsCollector> advertisementsCollectors, 
			ILogger logger)
		{
			_updateOffers = updateOffers;
			_executionHistoryRepository = executionHistoryRepository;
			_flatOffersRepository = flatOffersRepository;
			_advertisementsCollectors = advertisementsCollectors;
			_logger = logger;
		}

		public void Execute()
		{
			if (HasRunInPast24Hours())
			{
				return;
			}

			var started = DateTime.Now;

			try
			{
				UpdateFlattOffers();

				RecordExecutionFinished(started, true);
			}
			catch(Exception ex)
			{
				_logger.Error(ex, "Offer tracking failed");
				RecordExecutionFinished(started, false);
				throw;
			}
		}

		private void UpdateFlattOffers()
		{
			var advertisements = GetAdvertisements();
			var existingOffers = _flatOffersRepository.GetOpenFlatOffers();

			var resultingOffers = _updateOffers.Execute(new UpdateOffersBasedOnAdvertisementsCommand
			{
				Advertisements = advertisements,
				Offers = existingOffers
			});

			_flatOffersRepository.Save(resultingOffers);
		}

		private IEnumerable<Advertisement> GetAdvertisements()
		{
			var advertisements = new List<Advertisement>();
			foreach(var collector in _advertisementsCollectors)
			{
				advertisements.AddRange(collector.Collect());
			}
			
			return advertisements;
		}

		private bool HasRunInPast24Hours()
		{
			var lastExecution = _executionHistoryRepository.GetLatestExecutionRecord();

			return lastExecution == null ?
				false :
				HoursFromLastExecution(lastExecution) > 24;
		}

		private static int HoursFromLastExecution(ExecutionRecord lastExecution)
		{
			return (DateTime.Now - lastExecution.DateTimeFinished).Hours;
		}

		private void RecordExecutionFinished(DateTime started, bool succeded)
		{
			_executionHistoryRepository.Save(new ExecutionRecord
			{
				Success = succeded,
				DateTimeFinished = DateTime.Now,
				DateTimeStarted = started
			});
		}
	}
}
