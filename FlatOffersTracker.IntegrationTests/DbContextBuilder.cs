using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FlatOffersTracker.IntegrationTests
{
	public class DbContextBuilder
	{
		public T CreateDbContext<T>(Func<DbContextOptions<T>, T> create) where T : DbContext
		{			
			var serviceProvider = new ServiceCollection()
				.AddEntityFrameworkSqlServer()
				.BuildServiceProvider();

			var builder = new DbContextOptionsBuilder<T>();

			builder.UseSqlServer($"Server=localhost\\SQLEXPRESS;Database={typeof(T).Name}_{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=true",
				x => x.MigrationsAssembly(typeof(T).Assembly.GetName().Name))
			.UseInternalServiceProvider(serviceProvider);

			var context = create(builder.Options);			
			context.Database.Migrate();
			
			return context;
		}
	}
}
