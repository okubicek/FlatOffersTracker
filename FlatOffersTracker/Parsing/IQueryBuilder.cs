using System;
using System.Collections.Generic;
using System.Text;

namespace FlatOffersTracker.Parsing
{
	public interface IQueryBuilder
	{
		string GetQueryString(Query query);
	}
}
