using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions.EnumerableExtensions
{
	public static class IEnumerableExtensions
	{
		public static bool None<T>(this IEnumerable<T> list)
		{
			return !list.Any();
		}
	}
}
