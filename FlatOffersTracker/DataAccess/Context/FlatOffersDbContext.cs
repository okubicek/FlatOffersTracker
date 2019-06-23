using FlatOffersTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlatOffersTracker.DataAccess.Context
{
	public class FlatOffersDbContext : DbContext
	{
		public FlatOffersDbContext(DbContextOptions<FlatOffersDbContext> options) : base(options)
		{
		}

		public DbSet<FlatOffer> FlatOffers { get; set; }

		public DbSet<ExecutionRecord> Execution { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder
				.Entity<FlatOffer>()
				.ToTable("FlatOffers");
			AddPrimaryKey(builder
				.Entity<FlatOffer>());
			builder
				.Entity<FlatOffer>()
				.HasMany(x => x.Links)
				.WithOne(y => y.FlatOffer);

			builder.
				Entity<Link>()
				.ToTable("FlatOffersLinks");
			AddPrimaryKey(builder
				.Entity<Link>());

			builder
				.Entity<Notification>()
				.ToTable("Notificatins");
			AddPrimaryKey(builder
				.Entity<Notification>());
			builder.Entity<Notification>()
				.HasOne(x => x.FlatOffer)
				.WithMany(x => x.Notifications);

			builder
				.Entity<ExecutionRecord>()
				.ToTable("ExecutionHistory");
			AddPrimaryKey(builder
				.Entity<ExecutionRecord>());
		}

		private void AddPrimaryKey<T>(EntityTypeBuilder<T> builder) where T : class
		{
			builder
				.Property<int>("Id")
				.ValueGeneratedOnAdd();

			builder
				.HasKey("Id");
		}
	}
}
