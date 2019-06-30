using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using FlatOffersTrackerBackgroundApp.Bootstrap;
using FlatOffersTrackerBackgroundApp.Jobs;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Windsor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FlatOffersTrackerBackgroundApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = new HostBuilder()
			.UseServiceProviderFactory(new WindsorServiceProviderFactory())
			.ConfigureContainer<IWindsorContainer>((hostContext, container) =>
			{
				container.Register(Component.For<IAdvertisementTrackingJob>().ImplementedBy<AdvertisementTrackingJob>().LifestyleTransient());
				container.Register(Component.For<SqlServerStorageOptions>().Instance(new SqlServerStorageOptions
				{
					CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
					QueuePollInterval = TimeSpan.FromTicks(1),
					UseRecommendedIsolationLevel = true,
					SlidingInvisibilityTimeout = TimeSpan.FromMinutes(1)
				}));
				container.Install(new FlatOffersTrackerInstaller());

				GlobalConfiguration.Configuration.UseWindsorActivator(container.Kernel);
				//JobActivator.Current = new WindsorJobActivator(container.Kernel);
			})
			.ConfigureAppConfiguration((hostContext, config) =>
			{
				config.AddJsonFile("appsettings.json", true);
			})
			.ConfigureServices((hostContext, services) =>
			{
				services.AddHangfire((provider, configuration) => configuration
						.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
						.UseSimpleAssemblyNameTypeSerializer()
						.UseSqlServerStorage(
							@"Server=localhost\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;MultipleActiveResultSets=true",
							provider.GetRequiredService<SqlServerStorageOptions>()));

				services.AddHostedService<RecurringJobsService>();
				services.AddHangfireServer();
			})
			.Build();

			await host.RunAsync();
		}
	}
}
