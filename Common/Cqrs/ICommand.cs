namespace Common.Cqrs
{
	public interface ICommand
	{
		void Execute();
	}

	public interface ICommand<TReturn, TParam>
	{
		TReturn Execute(TParam command);
	}

	public interface ICommand<TParam>
	{
		void Execute(TParam command);
	}
}
