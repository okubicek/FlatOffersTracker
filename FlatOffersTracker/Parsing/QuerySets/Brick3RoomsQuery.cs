﻿namespace FlatOffersTracker.Parsing.QuerySets
{
	public class Brick3RoomsQuery : Query
	{
		public Brick3RoomsQuery()
		{
			FlatType = Entities.FlatType.Brick;
			NumberOfRooms = 3;
			PriceTopLimit = 5000000;
			Location = "Brno";
		}
	}
}