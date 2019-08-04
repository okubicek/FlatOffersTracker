using FlatOffersTracker.Entities;

namespace EFRepository.DataAccess.Repositories
{
	public interface IExecutionHistoryRepository
	{
		ExecutionRecord GetLatestExecutionRecord();

		void Save(ExecutionRecord record);
	}
}
