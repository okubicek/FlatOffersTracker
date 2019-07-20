using FlatOffersTracker.Cqrs;
using Serilog;
using System;

namespace FlatOffersTrackerBackgroundApp.Jobs
{
	public interface IAdvertisementTrackingJob
	{
		void Run();
	}

	public class AdvertisementTrackingJob : IAdvertisementTrackingJob
	{
		private ICommand _tracker;

		private ILogger _logger;

		public AdvertisementTrackingJob(ICommand tracker, ILogger logger)
		{
			_tracker = tracker;
			_logger = logger;
		}

		public void Run()
		{
			_logger.Information("Batch started at {Now}", DateTime.Now);

			_tracker.Execute();

			_logger.Information("Batch execution finished at {Now}", DateTime.Now);
		}
	}
}
