using FlatOffersTracker.DataAccess.Repositories;
using FlatOffersTracker.Entities;
using System;
using System.Collections.Generic;

namespace FlatOffersTracker
{
	public class TractOffersCommandHandler
	{
		private IExecutionHistoryRepository _executionHistoryRepository;

		private IFlatOffersRepository _flatOffersRepository;

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
				RecordExecutionFinished(started, false);
			}
		}

		private void UpdateFlattOffers()
		{
			var advertisements = GetAdvertisements();
			var offers = _flatOffersRepository.GetOpenFlatOffers();

			//Update offers based on Advertisements

			_flatOffersRepository.Save(offers);
		}

		private IEnumerable<Advertisement> GetAdvertisements()
		{
			return new List<Advertisement>();
		}

		private bool HasRunInPast24Hours()
		{
			var lastExecution = _executionHistoryRepository.GetLatestExecutionRecord();

			return lastExecution == null ?
				true :
				HoursFromLastExecution(lastExecution) > 24;
		}

		private static int HoursFromLastExecution(ExecutionRecord lastExecution)
		{
			return (DateTime.Now - lastExecution.DateTimeFinished).Hours;
		}

		private void RecordExecutionFinished(DateTime started, bool succeded)
		{
			_executionHistoryRepository.Save(new Entities.ExecutionRecord
			{
				Success = succeded,
				DateTimeFinished = DateTime.Now,
				DateTimeStarted = started
			});
		}
	}
}
