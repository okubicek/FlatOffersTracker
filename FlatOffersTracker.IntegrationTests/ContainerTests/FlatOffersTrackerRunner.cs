using FlatOffersTrackerBackgroundApp.Jobs;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace FlatOffersTracker.IntegrationTests.ContainerTests
{
	public class FlatOffersTrackerRunner : BackgroundService
	{
		private IAdvertisementTrackingJob _job;

		public FlatOffersTrackerRunner(IAdvertisementTrackingJob job)
		{
			_job = job;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			return Task.CompletedTask;
		}
	}
}
