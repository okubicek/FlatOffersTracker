using Common.Cqrs;
using EFRepository.DataAccess.Repositories;
using FlatOffersTracker.Entities;
using FlatOffersTrackerBackgroundApp.Jobs;
using Moq;
using Serilog;
using System;
using Xunit;

namespace FlatOffersTracker.Tests.AdvertisementTrackerJob
{
	public abstract class AdvertisementTrackerJobTestsBase
	{
		protected AdvertisementTrackingJob UnderTest;

		protected Mock<ICommand> MockedFlatOfferExecution;

		protected abstract ExecutionRecord LastExecution { get; }

		public AdvertisementTrackerJobTestsBase()
		{
			MockedFlatOfferExecution = new Mock<ICommand>();
			var mockedRepo = new Mock<IExecutionHistoryRepository>();
			mockedRepo.Setup(x => x.GetLatestExecutionRecord()).Returns(LastExecution);

			UnderTest = new AdvertisementTrackingJob(MockedFlatOfferExecution.Object, Log.Logger, mockedRepo.Object);

			UnderTest.Run();
		}
	}

	public class WhenRunningJobAfterLessThan24HoursSinceLastRun : AdvertisementTrackerJobTestsBase
	{
		protected override ExecutionRecord LastExecution => new ExecutionRecord
		{
			DateTimeFinished = DateTime.Now.AddHours(-5),
			DateTimeStarted = DateTime.Now.AddHours(-5),
			Success = true
		};

		[Fact]
		public void ExecutionShouldBeSkipped()
		{
			MockedFlatOfferExecution.Verify(x => x.Execute(), Times.Never());
		}
	}

	public class WhenRunningJobAfterLessThan24HoursSinceLastFailedRun : AdvertisementTrackerJobTestsBase
	{
		protected override ExecutionRecord LastExecution => new ExecutionRecord
		{
			DateTimeFinished = DateTime.Now.AddHours(-25),
			DateTimeStarted = DateTime.Now.AddHours(-25),
			Success = true
		};

		[Fact]
		public void ExecutionShouldBeSkipped()
		{
			MockedFlatOfferExecution.Verify(x => x.Execute(), Times.Once());
		}
	}
}
