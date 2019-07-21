using FlatOffersTracker.Entities;

namespace FlatOffersTrackerBackgroundApp.DataAccess.Repositories
{
	public interface IExecutionHistoryRepository
	{
		ExecutionRecord GetLatestExecutionRecord();

		void Save(ExecutionRecord record);
	}
}
