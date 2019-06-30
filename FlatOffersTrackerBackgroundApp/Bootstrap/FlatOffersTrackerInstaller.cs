using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FlatOffersTracker;
using FlatOffersTracker.Cqrs;

namespace FlatOffersTrackerBackgroundApp.Bootstrap
{
	public class FlatOffersTrackerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component
				.For<ICommandHandler>()
				.ImplementedBy<TractOffersCommandHandler>()
				.LifestyleTransient());

			container.Register(Classes
				.FromAssemblyContaining<TractOffersCommandHandler>()
				.BasedOn(typeof(ICommandHandler<,>))
				.WithService
				.Base()
				.LifestyleTransient());
		}
	}
}
