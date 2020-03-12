using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Threading.Tasks;

namespace FlatOffersTracker.IntegrationTests.Helpers
{
	public static class FlatOffersDbContextSetupMethods
	{
		public static IFlatOffersDbContext ResetDb(this IFlatOffersDbContext setup)
		{
			var connectionString = setup.DbContext.Database.GetDbConnection().ConnectionString;
			var checkpoint = new Checkpoint();
			Task.Run(() => checkpoint.Reset(connectionString)).Wait();

			return setup;
		}
	}
}
