using FlatOffersTracker.Entities;

namespace FlatOffersTracker.DataAccess
{
	public interface IExecutionHistoryRepository
	{
		ExecutionRecord GetLatestExecutionRecord();

		void Save(ExecutionRecord record);
	}
}
