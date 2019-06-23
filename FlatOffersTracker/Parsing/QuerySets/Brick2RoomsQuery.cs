namespace FlatOffersTracker.Parsing.QuerySets
{
	public class Brick2RoomsQuery : Query
	{
		public Brick2RoomsQuery()
		{
			FlatType = Entities.FlatType.Brick;
			NumberOfRooms = 2;
			PriceTopLimit = 3500000;
		}
	}
}
