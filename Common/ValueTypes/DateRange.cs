using System;
using System.Collections.Generic;

namespace Common.ValueTypes
{
	public struct DateRange
	{
		public DateRange(DateTime? startDate, DateTime? endDate) : this()
		{
			if (startDate == null && endDate == null)
			{
				throw new ArgumentException("At least one of the pair StartDate, EndDate must be specified");
			}

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
