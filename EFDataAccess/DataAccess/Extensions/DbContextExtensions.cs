using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EFRepository.DataAccess
{
	public static class DbContextExtensions
	{
		public static void FixStateOfEntitiesWithoutKey<T>(this DbContext context, IEnumerable<T> entities) where T : class
		{
			var addedEntities = entities.Where(x => !context.Entry(x).IsKeySet);
			
			foreach(var e in addedEntities)
			{
				context.Entry(e).State = EntityState.Added;
			}
		}
	}
}
