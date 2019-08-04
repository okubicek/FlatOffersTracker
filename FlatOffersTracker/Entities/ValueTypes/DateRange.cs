using System;
using System.Collections.Generic;

namespace FlatOffersTracker.Entities.ValueTypes
{
	public struct DateRange
	{
		public DateRange(DateTime? startDate, DateTime? endDate) : this()
		{
			StartDate = startDate;
			EndDate = endDate;
		}

		public DateTime? StartDate { get; }

		public DateTime? EndDate { get; }

		public override bool Equals(object obj)
		{
			if (!(obj is DateRange))
			{
				return false;
			}

			var range = (DateRange)obj;
			return EqualityComparer<DateTime?>.Default.Equals(StartDate, range.StartDate) &&
				   EqualityComparer<DateTime?>.Default.Equals(EndDate, range.EndDate);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(StartDate, EndDate);
		}
	}
}
