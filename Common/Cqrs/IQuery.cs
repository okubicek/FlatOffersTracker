namespace Common.Cqrs
{
	public interface IQuery<TReturn, TParam>
	{
		TReturn Get(TParam query);
	}

	public interface IQuery<TReturn>
	{
		TReturn Get();
	}
}