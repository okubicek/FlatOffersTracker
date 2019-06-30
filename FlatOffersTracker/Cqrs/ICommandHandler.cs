using System;
using System.Collections.Generic;
using System.Text;

namespace FlatOffersTracker.Cqrs
{
	public interface ICommandHandler
	{
		void Execute();
	}

	public interface ICommandHandler<TReturn, TParam>
	{
		TReturn Execute(TParam command);
	}
}
