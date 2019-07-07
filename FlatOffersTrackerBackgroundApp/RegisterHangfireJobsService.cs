using FlatOffersTrackerBackgroundApp.Jobs;
using Hangfire;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace FlatOffersTrackerBackgroundApp
{
	public class RegisterHangfireJobsService : BackgroundService
	{
		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			RecurringJob.AddOrUpdate<IAdvertisementTrackingJob>(x => x.Run(), Cron.Daily(20));

			return Task.CompletedTask;
		}
	}
}
