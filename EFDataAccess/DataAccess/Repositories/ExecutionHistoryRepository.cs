using EFRepository.DataAccess.Context;
using FlatOffersTracker.Entities;
using System.Linq;

namespace EFRepository.DataAccess.Repositories
{
	public class ExecutionHistoryRepository : IExecutionHistoryRepository
	{
		private FlatOffersDbContext _dbContext;

		public ExecutionHistoryRepository(FlatOffersDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public ExecutionRecord GetLatestExecutionRecord()
		{
			return _dbContext.Execution
				.Where(x => x.Success)
				.OrderByDescending(x => x.DateTimeFinished)
				.FirstOrDefault();
		}

		public void Save(ExecutionRecord record)
		{
			_dbContext.Add(record);
			_dbContext.SaveChanges();
		}
	}
}
