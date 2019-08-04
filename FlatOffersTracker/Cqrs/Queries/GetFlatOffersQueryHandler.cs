using Common.Cqrs;
using FlatOffersTracker.Entities;
using System;
using System.Collections.Generic;

namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetFlatOffersQueryHandler : IQueryHandler<IEnumerable<FlatOffer>, GetFlatOffersQuery>
	{
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public IEnumerable<FlatOffer> Get(GetFlatOffersQuery query)
		{
			throw new NotImplementedException();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
