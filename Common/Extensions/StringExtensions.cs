using System;

namespace Common.Extensions
{
	public static class StringExtensions
	{
		public static T ToEnum<T>(this string str) where T : Enum
		{
			return (T)Enum.Parse(typeof(T), str, true);
		}

	}
}
