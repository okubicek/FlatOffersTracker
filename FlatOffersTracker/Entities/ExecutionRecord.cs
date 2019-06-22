using System;

namespace FlatOffersTracker.Entities
{
	public class ExecutionRecord
	{
		public bool Success { get; set; }

		public DateTime DateTimeStarted { get; set; }

		public DateTime DateTimeFinished { get; set; }
	}
}
