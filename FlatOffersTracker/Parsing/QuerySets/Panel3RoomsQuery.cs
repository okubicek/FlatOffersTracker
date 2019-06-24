using System;
using System.Collections.Generic;
using System.Text;

namespace FlatOffersTracker.Parsing.QuerySets
{
	public class Panel3RoomsQuery : Query
	{
		public Panel3RoomsQuery()
		{
			FlatType = Entities.FlatType.Panel;
			NumberOfRooms = 3;
			PriceTopLimit = 4500000;
			Location = "Brno";
		}
	}
}
