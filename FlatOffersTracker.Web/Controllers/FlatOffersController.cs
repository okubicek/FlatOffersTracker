using Common.Cqrs;
using FlatOffersTracker.Cqrs.Queries;
using FlatOffersTracker.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker.Web.Controllers
{
	[Route("api/[controller]")]
	public class FlatOffersController : Controller
    {
		private IQuery<IEnumerable<FlatOffer>, GetFlatOffersQuery> _getFlatOffers;

		public FlatOffersController(IQuery<IEnumerable<FlatOffer>, GetFlatOffersQuery> getFlatOffers)
		{
			_getFlatOffers = getFlatOffers;
		}

		[HttpGet("[action]")]
		public IEnumerable<Models.FlatOffer> Get()
        {
			var offers = _getFlatOffers.Get(new GetFlatOffersQuery());
			return offers.Select(x => new Models.FlatOffer {
				Address = x.Address,
				NumberOfRooms = x.NumberOfRooms,
				FlatSize = x.FlatSize,
				FlatType = x.FlatType.ToString(),
				Price = x.Price,
				Url = x.Links.First().Url
			});
        }
    }
}