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
		private ICommand _tracker;

		public AdvertisementTrackingJob(ICommand tracker)
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
