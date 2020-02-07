using Common.Cqrs;
using Common.Extensions;
using FlatOffersTracker.Cqrs.Queries;
using FlatOffersTracker.Entities;
using FlatOffersTracker.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker.Web.Controllers
{
	[Route("api/[controller]")]
	public class FlatOffersController : Controller
	{
		private IQuery<IEnumerable<FlatOffer>, GetFlatOffersQuery> _getFlatOffers;

		private IQuery<IEnumerable<byte[]>, GetFlatOfferImagesQuery> _getImages;

		public FlatOffersController(IQuery<IEnumerable<FlatOffer>, GetFlatOffersQuery> getFlatOffers,
			IQuery<IEnumerable<byte[]>, GetFlatOfferImagesQuery> getImages)
		{
			_getFlatOffers = getFlatOffers;
			_getImages = getImages;
		}

		[HttpGet("[action]")]
		public IEnumerable<Models.FlatOffer> Get(Models.FlatOffersSearchParams query)
		{
			var offers = _getFlatOffers.Get(new GetFlatOffersQuery {
				FlatType = query.FlatType?.ToEnum<FlatType>(),
				MaxPrice = query.MaxPrice,
				MinFlatSize = query.MinFlatSize,
				NumberOfRooms = query.RoomCount
			});

			return offers.Select(x => new Models.FlatOffer {
				Id = x.Id.Value,
				Address = x.Address,
				NumberOfRooms = x.NumberOfRooms,
				FlatSize = x.FlatSize,
				FlatType = x.FlatType.ToString(),
				Price = x.Price,
				Url = x.Links.First().Url
			});
		}

		[HttpGet("[action]")]
		public IEnumerable<Models.SelectOption> Definitions(string definitionType)
		{
			Enum.TryParse(definitionType, out Models.DefinitionTypes defType);

			switch (defType)
			{
				case Models.DefinitionTypes.FlatType:
					return EnumHelper.ConvertEnumtToSelectList<FlatType>();
				case Models.DefinitionTypes.NumberOfRooms:
					return new List<Models.SelectOption> { new Models.SelectOption(1, "2"), new Models.SelectOption(2, "3") };
				default:
					throw new ArgumentException($"Value {definitionType} is not supported definition type");
			}
		}

		[HttpGet("[action]/{flatOfferId}")]
		public ActionResult HeaderImage([FromRoute]int flatOfferId)
		{
			var image = _getImages.Get(new GetFlatOfferImagesQuery
			{
				FlatOfferId = flatOfferId,
				ReturnHeadImageOnly = true
			})
			.Single();

			return File(image, "image/jpg");
		}
	}
}