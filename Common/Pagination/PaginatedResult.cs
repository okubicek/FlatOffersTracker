using System.Collections.Generic;

namespace Common.Pagination
{
	public class PaginatedResult<T>
	{
		public PaginatedResult(int totalRowCount, int pageNumber, int pageSize, IList<T> result)
		{
			TotalRowCount = totalRowCount;
			PageNumber = pageNumber;
			PageSize = pageSize;
			Results = result;
		}

		public int TotalRowCount { get; private set; }

		public int PageNumber { get; private set; }

		public int PageSize { get; private set; }

		public IList<T> Results { get; private set; }
	}
}