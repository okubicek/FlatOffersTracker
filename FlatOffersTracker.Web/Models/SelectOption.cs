using System.Collections.Generic;

namespace FlatOffersTracker.Web.Models
{
	public class SelectOption
	{
		public SelectOption(int key, string value)
		{
			Value = value;
			Key = key;
		}

		public string Value { get; private set; }

		public int Key { get; private set; }
	}
}
