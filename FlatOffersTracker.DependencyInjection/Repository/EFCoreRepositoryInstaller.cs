using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EFRepository.DataAccess.Context;
using EFRepository.DataAccess.Repositories;
using FlatOffersTracker.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlatOffersTracker.DependencyInjection.Repository
{
	public class EFCoreRepositoryInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component
				.For<IFlatOffersRepository>()
				.ImplementedBy<FlatOffersRepository>()
				.LifestyleTransient());
			
			container.Register(Component
				.For<INotificationRepository>()
				.ImplementedBy<NotificationRepository>()
				.LifestyleTransient());			

			container.Register(Component
				.For<IExecutionHistoryRepository>()
				.ImplementedBy<ExecutionHistoryRepository>()
				.LifestyleTransient());
		}

		public void AddToServiceCollection(IServiceCollection services, string connectionString)
		{
			services.AddDbContext<FlatOffersDbContext>(options =>
						options.UseSqlServer(connectionString,
							ob => ob.MigrationsAssembly(typeof(FlatOffersDbContext).Assembly.FullName))
					);
		}
	}
}
