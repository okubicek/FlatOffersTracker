using Castle.Windsor;
using Common.Cqrs;
using FlatOffersTracker.DependencyInjection.Domain;
using FlatOffersTracker.DependencyInjection.Repository;
using Microsoft.Extensions.Configuration;
using System;
using Castle.Facilities.Logging;
using Castle.Services.Logging.SerilogIntegration;
using Serilog;
using Serilog.Events;
using Castle.MicroKernel.Registration;

namespace FlatOffersTrackerConsoleApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			ConfigureLogger();
			var container = RegisterDependencies();

			var trackerRunner = container.Resolve<ICommand>();			

			trackerRunner.Execute();
			Console.WriteLine("Execution has finished. Pleare press any key:");
			Console.Read();
		}

		private static WindsorContainer RegisterDependencies()
		{
			var container = new WindsorContainer();			
			container.Install(
				new FlatOffersTrackerInstaller(),
				new EFCoreRepositoryInstaller()
			);
			
			//container.AddFacility<LoggingFacility>(f => f.LogUsing<SerilogFactory>());
			IConfiguration Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.Build();

			container.Register(Component.For<ILogger>().Instance(Log.Logger));

			var connectionString = Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
			container.AddTransientDdContext(connectionString);

			return container;
		}

		private static void ConfigureLogger()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.WriteTo.RollingFile("Logs\\Log{Date}.txt", outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
				.CreateLogger();
		}
	}
}
