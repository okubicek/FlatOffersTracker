using Common.Cqrs;
using EFRepository.DataAccess.Context;
using EFRepository.DataAccess.Repositories;
using FlatOffersTracker.Cqrs.Queries;
using FlatOffersTracker.IntegrationTests.BaseFixtures;
using FlatOffersTrackerBackgroundApp.Jobs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Respawn;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlatOffersTracker.IntegrationTests.AdvertisementTrackingJobTests
{
	[Collection(FlatOffersDbContextFixtureDefinition.Definition)]
	public class AdvertisementTrackingJobTest
	{
		protected FlatOffersDbContext Context;

		protected AdvertisementTrackingJob UnderTest;

		protected Mock<ICommand> MockedFlatOfferExecution;

		public AdvertisementTrackingJobTest(FlatOffersDbContextFixture dbFixture)
		{
			var connectionString = dbFixture.Context.Database.GetDbConnection().ConnectionString;
			var checkpoint = new Checkpoint();
			Task.Run(() => checkpoint.Reset(connectionString)).Wait();

			Context = dbFixture.Context;
			MockedFlatOfferExecution = new Mock<ICommand>();
			UnderTest = new AdvertisementTrackingJob(MockedFlatOfferExecution.Object, Log.Logger, new ExecutionHistoryRepository(Context));
		}

		public void CreateExecutionRecord(DateTime dateTimeStarted, DateTime dateTimeFinished, bool success)
		{
			Context.Execution.Add(new Entities.ExecutionRecord
			{
				DateTimeStarted = dateTimeStarted,
				DateTimeFinished = dateTimeFinished,
				Success = success
			});

			Context.SaveChanges();
		}
	}

	public class WhenRunningJobAfterLessThan24HoursSinceLastFailedRun : AdvertisementTrackingJobTest
	{
		public WhenRunningJobAfterLessThan24HoursSinceLastFailedRun(FlatOffersDbContextFixture dbFixture) : base(dbFixture)
		{
			CreateExecutionRecord(DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-5), false);
			CreateExecutionRecord(DateTime.Now.AddHours(-25), DateTime.Now.AddHours(-25), true);

			UnderTest.Run();
		}

		[Fact]
		public void ThereShouldBeThreeRecordsInExecutionHistory()
		{
			Assert.Equal(3, Context.Execution.Count());
		}
	}
}
