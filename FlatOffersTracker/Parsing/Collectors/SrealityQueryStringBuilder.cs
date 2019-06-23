using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace FlatOffersTracker.Parsing.Collectors
{
	public class SrealityQueryStringBuilder : IQueryBuilder
	{
		private string uri = "";

		public string GetQueryString(Query query)
		{
			var parameters = GetQueryParamsDictionary(query);
			var queryString = QueryHelpers.AddQueryString(uri, parameters);

			return queryString;
		}

		private Dictionary<string, string> GetQueryParamsDictionary(Query query)
		{
			var dict = new Dictionary<string, string>();

			dict.Add("", query.Location);
			dict.Add("", query.FlatType.ToString());
			dict.Add("", query.NumberOfRooms.ToString());

			if (query.PriceTopLimit.HasValue)
			{
				dict.Add("", query.PriceTopLimit.Value.ToString());
			}

			return dict;
		}
	}
}
