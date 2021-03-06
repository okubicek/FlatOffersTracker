﻿using Common.Extensions;
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
		private IGetFlatOffersHandler _getFlatOffers;

		private IGetFlatOfferImagesHandler _getImages;

		public FlatOffersController(IGetFlatOffersHandler getFlatOffers,
			IGetFlatOfferImagesHandler getImages)
		{
			_getFlatOffers = getFlatOffers;
			_getImages = getImages;
		}

		[HttpGet("[action]")]
		public Models.PaginatedCollection<Models.FlatOffer> Get(Models.FlatOffersSearchParams query)
		{
			var offers = _getFlatOffers.Get(new GetFlatOffersQuery {
				FlatType = query.FlatType?.ToEnum<FlatType>(),
				MaxPrice = query.MaxPrice,
				MinFlatSize = query.MinFlatSize,
				NumberOfRooms = query.RoomCount,
				Pagination = new Common.ValueTypes.QueryPagination(query.PageNumber, query.PageSize)
			});

			return new Models.PaginatedCollection<Models.FlatOffer>
			{
				Results = offers.Results.Select(x => new Models.FlatOffer
				{
					Id = x.Id.Value,
					Address = x.Address,
					NumberOfRooms = x.NumberOfRooms,
					FlatSize = x.FlatSize,
					FlatType = x.FlatType.ToString(),
					Price = x.Price,
					NotificationType = NotificationResolver.ResolveNotification(x.Notifications),
					Url = x.Links.First().Url
				}),
				PageNumber = offers.PageNumber,
				PageCount = offers.PageCount
			};
		}

		[HttpGet("[action]")]
		public IEnumerable<Models.SelectOption> Definitions(string definitionType)
		{
			Enum.TryParse(definitionType, out Models.DefinitionTypes defType);

			switch (defType)
			{
				case Models.DefinitionTypes.FlatType:
					return EnumHelper.ConvertEnumToSelectList<FlatType>();
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