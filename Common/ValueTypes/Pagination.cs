using System;

namespace Common.ValueTypes
{
	public class Pagination
	{
		public Pagination(int page, int pageSize)
		{
			if (page < 1) throw new ArgumentException("Page number can't be less than 0");

			Page = page;
			PageSize = pageSize;
		}

		public int Page { get; private set; }

		public int PageSize { get; private set; }
	}
}
