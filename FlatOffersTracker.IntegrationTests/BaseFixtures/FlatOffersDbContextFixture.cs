using EFRepository.DataAccess.Context;
using System;
using Xunit;

namespace FlatOffersTracker.IntegrationTests.BaseFixtures
{
	[CollectionDefinition(Definition)]
	public class FlatOffersDbContextFixtureDefinition
		: ICollectionFixture<FlatOffersDbContextFixture>
	{
		public const string Definition = nameof(FlatOffersDbContextFixture);
	}

	public class FlatOffersDbContextFixture : IDisposable
	{
		public FlatOffersDbContextFixture()
		{
			Context = new DbContextBuilder().CreateDbContext<FlatOffersDbContext>(opt => new FlatOffersDbContext(opt));
		}

		public FlatOffersDbContext Context { get; set; }

		public void Dispose()
		{
			Context.Database.EnsureDeleted();
		}
	}
}
