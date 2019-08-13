using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Cqrs;
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
				.ImplementedBy<TractOffersCommandHandler>()
				.LifestyleTransient());

			container.Register(Classes
				.FromAssemblyContaining<TractOffersCommandHandler>()
				.BasedOn(typeof(ICommand<,>))
				.OrBasedOn(typeof(IQuery<,>))
				.WithService
				.Base()
				.LifestyleTransient());

			container.Register(Classes
				.FromAssemblyContaining<TractOffersCommandHandler>()
				.BasedOn<IAdvertisementsCollector>()
				.WithService.FromInterface());
		}
	}
}
