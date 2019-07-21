using FlatOffersTracker.Cqrs;
using FlatOffersTracker.Entities;
using FlatOffersTrackerBackgroundApp.DataAccess.Repositories;
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
		private IExecutionHistoryRepository _executionHistoryRepository;

		private ICommand _tracker;

		private ILogger _logger;

		public AdvertisementTrackingJob(ICommand tracker, 
			ILogger logger, 
			IExecutionHistoryRepository executionHistoryRepository
		)
		{
			_logger = logger;
			_tracker = tracker;
			_executionHistoryRepository = executionHistoryRepository;
		}

		public void Run()
		{
			_logger.Information("Batch started at {Now}", DateTime.Now);

			if (HasRunInPast24Hours())
			{
				return;
			}

			var started = DateTime.Now;

			try
			{
				_tracker.Execute();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Offer tracking failed");
			}
			finally
			{
				RecordExecutionFinished(started, false);
			}

			_logger.Information("Batch execution finished at {Now}", DateTime.Now);
		}

		private bool HasRunInPast24Hours()
		{
			var lastExecution = _executionHistoryRepository.GetLatestExecutionRecord();

			return lastExecution == null ?
				false :
				HoursFromLastExecution(lastExecution) > 24;
		}

		private static int HoursFromLastExecution(ExecutionRecord lastExecution)
		{
			return (DateTime.Now - lastExecution.DateTimeFinished).Hours;
		}

		private void RecordExecutionFinished(DateTime started, bool succeded)
		{
			_executionHistoryRepository.Save(new ExecutionRecord
			{
				Success = succeded,
				DateTimeFinished = DateTime.Now,
				DateTimeStarted = started
			});
		}
	}
}