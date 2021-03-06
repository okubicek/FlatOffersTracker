﻿using Common.Cqrs;
using FlatOffersTracker.DataAccess;
using FlatOffersTracker.Entities;
using FlatOffersTracker.Parsing;
using Serilog;
using System;
using System.Collections.Generic;

namespace FlatOffersTracker.Cqrs.Commands
{
	public class TractOffersHandler : ICommand
	{
		private IFlatOffersRepository _flatOffersRepository;

		private IUpdateOffersBasedOnAdvertisementsHandler _updateOffers;

		private IEnumerable<IAdvertisementsCollector> _advertisementsCollectors;

		private ILogger _logger;

		public TractOffersHandler(IFlatOffersRepository flatOffersRepository,
			IUpdateOffersBasedOnAdvertisementsHandler updateOffers,
			IEnumerable<IAdvertisementsCollector> advertisementsCollectors, 
			ILogger logger)
		{
			_logger = logger;
			_updateOffers = updateOffers;
			_flatOffersRepository = flatOffersRepository;
			_advertisementsCollectors = advertisementsCollectors;
		}

		public void Execute()
		{
			try
			{
				UpdateFlattOffers();
			}
			catch(Exception ex)
			{
				_logger.Error(ex, "Offer tracking failed");
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
	}
}
