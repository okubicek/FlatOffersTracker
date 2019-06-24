using FlatOffersTracker.Entities;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;

namespace FlatOffersTracker.Parsing.Collectors
{
	public class SrealityQueryStringBuilder : IQueryBuilder
	{
		private string uri = "https://www.sreality.cz/hledani/prodej/byty/";

		public string GetQueryString(Query query)
		{
			var url = uri + $"/{query.Location}";
			var parameters = GetQueryParamsDictionary(query);
			var queryString = QueryHelpers.AddQueryString(uri, parameters);

			return queryString;
		}

		private Dictionary<string, string> GetQueryParamsDictionary(Query query)
		{
			var dict = new Dictionary<string, string>();

			dict.Add("stavba", ConvertFlatType(query.FlatType));
			dict.Add("velikost", ConvertNumberOfRooms(query.NumberOfRooms));

			if (query.PriceTopLimit.HasValue)
			{
				dict.Add("cena-do", query.PriceTopLimit.Value.ToString());
			}

			return dict;
		}

		private string ConvertFlatType(FlatType flatType)
		{
			switch (flatType)
			{ 
				case FlatType.Brick:
					return "cihlova";
				case FlatType.Panel:
					return "panel";
				default:
					throw new NotImplementedException($"Flat type {flatType.ToString()} is not supported");
			}
		}

		private string ConvertNumberOfRooms(int numberOfRooms)
		{
			switch(numberOfRooms)
			{
				case 1:
					return "1+1";
				case 2:
					return "2+1";
				case 3:
					return "3+1,3+kk";
				case 4:
					return "4+1,4+kk";
				default:
					throw new NotImplementedException($"Number of rooms {numberOfRooms} is not supported");
			}
		}
	}
}
