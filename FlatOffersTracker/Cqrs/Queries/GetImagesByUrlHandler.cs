using Common.Cqrs;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetImagesByUrlHandler : IQuery<Dictionary<long, List<byte[]>>, GetImagesByUrlQuery>
	{		
		public Dictionary<long, List<byte[]>> Get(GetImagesByUrlQuery query)
		{
			var dict = new Dictionary<long, List<byte[]>>();

			using (var client = new WebClient())
			{
				foreach(var imageRequest in query.Requests.SelectMany(x => x.Urls, (Parent, Url) => new { flatOffer = Parent.Id, Url }))
				{
					var rawData = client.DownloadData(imageRequest.Url);
					if (!dict.ContainsKey(imageRequest.flatOffer))
					{
						var l = new List<byte[]>();
						dict.Add(imageRequest.flatOffer, l);
					}

					var images = dict[imageRequest.flatOffer];
					images.Add(rawData);
				}
			}

			return dict;
		}
	}
}
