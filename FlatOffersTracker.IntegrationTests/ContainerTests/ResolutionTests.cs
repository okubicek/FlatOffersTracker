using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FlatOffersTracker.IntegrationTests.ContainerTests
{
	public class ResolutionTests
	{
		[Fact]
		public async Task AdvertisementTrackingJobCanBeResolved()
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
}
