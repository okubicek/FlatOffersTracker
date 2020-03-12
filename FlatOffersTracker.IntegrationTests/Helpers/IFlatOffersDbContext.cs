using EFRepository.DataAccess.Context;

namespace FlatOffersTracker.IntegrationTests.Helpers
{
	public interface IFlatOffersDbContext
	{
		FlatOffersDbContext DbContext { get; }
	}
}
