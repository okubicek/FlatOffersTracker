using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.EntityFrameworkCore;
using EFRepository.DataAccess.Context;
using System;

namespace FlatOffersTracker.DependencyInjection.Repository
{
	public static class WindsorContainerExtension
	{
		public static void AddScopedDdContext(this WindsorContainer container, string connectionString)
		{
			Func<FlatOffersDbContext> create = () => 
				new FlatOffersDbContext(new DbContextOptionsBuilder<FlatOffersDbContext>()
				.UseSqlServer(connectionString, ob => ob.MigrationsAssembly(typeof(FlatOffersDbContext).Assembly.FullName))
				.Options);

			container.Register(Component
				.For<FlatOffersDbContext>()
				.UsingFactoryMethod(x => create())
				.LifestyleScoped());
		}
	}
}
