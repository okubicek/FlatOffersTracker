using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EFRepository.DataAccess
{
	public static class DbContextExtensions
	{
		public static void FixStateOfEntitiesWithoutKey<T>(this DbContext context, IEnumerable<T> entities) where T : class
		{			
			foreach(var e in entities)
			{
				var entry = context.Entry(e);
				if (!entry.IsKeySet)
				{
					context.Entry(e).State = EntityState.Added;
				}
			}
		}
	}
}
