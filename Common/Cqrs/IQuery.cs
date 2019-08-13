namespace Common.Cqrs
{
	public interface IQuery<TReturn, TParam>
	{
		TReturn Get(TParam query);
	}
}