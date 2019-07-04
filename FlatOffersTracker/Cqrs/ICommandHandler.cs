using System;
using System.Collections.Generic;
using System.Text;

namespace FlatOffersTracker.Cqrs
{
	public interface ICommand
	{
		void Execute();
	}

	public interface ICommand<TReturn, TParam>
	{
		TReturn Execute(TParam command);
	}
}
