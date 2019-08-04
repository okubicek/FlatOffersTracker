namespace Common.Cqrs
{
	public interface IQueryHandler<TReturn, TParam>
	{
		TReturn Get(TParam query);
	}
}
