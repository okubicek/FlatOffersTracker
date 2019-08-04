using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FlatOffersTracker;
using Common.Cqrs;
using FlatOffersTracker.DataAccess;
using FlatOffersTracker.Parsing;
using EFRepository.DataAccess.Repositories;

namespace FlatOffersTrackerBackgroundApp.Bootstrap
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
				.WithService
				.Base()
				.LifestyleTransient());

			container.Register(Component
				.For<IFlatOffersRepository>()
				.ImplementedBy<FlatOffersRepository>()
				.LifestyleTransient());

			container.Register(Component
				.For<IExecutionHistoryRepository>()
				.ImplementedBy<ExecutionHistoryRepository>()
				.LifestyleTransient());

			container.Register(Classes
				.FromAssemblyContaining<TractOffersCommandHandler>()
				.BasedOn<IAdvertisementsCollector>()
				.WithService.FromInterface());
		}
	}
}
