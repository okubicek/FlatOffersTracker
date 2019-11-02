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
		public async Task Test1()
		{
			var host = FlatOffersTrackerBackgroundApp.Program.CreateWebHostBuilder(new string[0]);
			host.ConfigureServices((hostContext, services) =>
			{
				services.AddHostedService<FlatOffersTrackerRunner>();
			});

			var buildedHost = host.Build();
			var service = buildedHost.Services.GetRequiredService<IHostedService>();
			await service.StartAsync(new CancellationToken());
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
