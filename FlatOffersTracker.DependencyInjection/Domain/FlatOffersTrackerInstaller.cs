using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Cqrs;
using FlatOffersTracker.Cqrs.Commands;
using FlatOffersTracker.Parsing;

namespace FlatOffersTracker.DependencyInjection.Domain
{
	public class FlatOffersTrackerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, false));

			container.Register(Component
				.For<ICommand>()
				.ImplementedBy<TractOffersHandler>()
				.LifestyleTransient());

			container.Register(Classes
				.FromAssemblyContaining<TractOffersHandler>()
				.BasedOn(typeof(ICommand<,>))
				.OrBasedOn(typeof(ICommand<>))
				.OrBasedOn(typeof(IQuery<,>))
				.OrBasedOn(typeof(IQuery<>))
				.WithService
				.AllInterfaces()
				.LifestyleTransient());

			container.Register(Classes
				.FromAssemblyContaining<TractOffersHandler>()
				.BasedOn<IAdvertisementsCollector>()
				.WithService.FromInterface());
		}
	}
}
