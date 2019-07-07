using FlatOffersTrackerBackgroundApp.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FlatOffersTracker.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var host = FlatOffersTrackerBackgroundApp.Program.CreateWebHostBuilder(new string[0]);
			host.ConfigureServices((hostContext, services) =>
			{
				services.AddHostedService<FlatOffersTrackerRunner>();
			});

			host.Build().StartAsync();
		}
	}

	public class FlatOffersTrackerRunner : BackgroundService
	{
		private IAdvertisementTrackingJob _job;

		public FlatOffersTrackerRunner(IAdvertisementTrackingJob job)
		{
			_job = job;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_job.Run();
			return Task.CompletedTask;
		}
	}
}
