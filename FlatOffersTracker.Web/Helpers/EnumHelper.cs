using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FlatOffersTracker.Web.Helpers
{
	public class EnumHelper
	{
		public static IEnumerable<Models.SelectOption> ConvertEnumtToSelectList<T>() where T : IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enum");
			}

			var enumValues = Enum.GetValues(typeof(T))
					.Cast<T>()
					.Where(x => !x.ToString().Equals("NotSpecified"));

			return enumValues.Select(x => new Models.SelectOption(x.ToInt32(CultureInfo.CurrentCulture), x.ToString()));
		}
	}
}
