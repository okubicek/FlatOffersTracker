using System.Collections.Generic;

namespace FlatOffersTracker
{
	public static class HashSetExtensions
	{
		public static void AddIfNotExists<T>(this HashSet<T> set, T value)
		{
			if (!set.Contains(value))
			{
				set.Add(value);
			}
		}
	}
}
