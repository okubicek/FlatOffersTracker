using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using FlatOffersTrackerBackgroundApp.Bootstrap;
using FlatOffersTrackerBackgroundApp.DataAccess.Context;
using FlatOffersTrackerBackgroundApp.Jobs;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Windsor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FlatOffersTrackerBackgroundApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

			await host.RunAsync();
		}

		public static IHostBuilder CreateWebHostBuilder(string[] args) // calling this WebHostBuilder so entity framework is able to create migraitons
		{
			var builder = new HostBuilder()
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
				.ConfigureHostConfiguration(configuration => 
				{

					configuration.SetBasePath(Directory.GetCurrentDirectory());
					configuration.AddJsonFile("appsettings.json", true);
				})
				.ConfigureServices((hostContext, services) =>
				{
					var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
					services.AddDbContext<FlatOffersDbContext>(options =>
						options.UseSqlServer(connectionString)
					);

					services.AddHangfire((provider, configuration) => configuration
							.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
							.UseSimpleAssemblyNameTypeSerializer()
							.UseSqlServerStorage(
								connectionString,
								provider.GetRequiredService<SqlServerStorageOptions>()));

					services.AddHostedService<RecurringJobsService>();
					services.AddHangfireServer();
				});

			return builder;
		}
	}
}