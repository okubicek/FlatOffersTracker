using FlatOffersTracker.Cqrs;
using System;

namespace FlatOffersTrackerBackgroundApp.Jobs
{
	public interface IAdvertisementTrackingJob
	{
		void Run();
	}

	public class AdvertisementTrackingJob : IAdvertisementTrackingJob
	{
		private ICommandHandler _tracker;

		public AdvertisementTrackingJob(ICommandHandler tracker)
		{
			_tracker = tracker;
		}

		public void Run()
		{
			_tracker.Execute();

			Console.Write($"Batch has been executed at {DateTime.Now}");
		}
	}
}
