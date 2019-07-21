using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTrackerBackgroundApp.DataAccess
{
	public static class DbContextExtensions
	{
		public static void FixStateOfEntitiesWithoutKey<T>(this DbContext context, IEnumerable<T> entities)
		{
			var addedEntities = entities.Where(x => !context.Entry(x).IsKeySet);
			
			foreach(var e in addedEntities)
			{
				context.Entry(e).State = EntityState.Added;
			}
		}
	}
}
