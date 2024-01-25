using Microsoft.EntityFrameworkCore;
using NBDProjectNcstech.Models;

namespace NBDProjectNcstech.Data
{
	public class NBDContext : DbContext
	{
		public NBDContext(DbContextOptions<NBDContext> options) : base(options)
		{

		}

		public DbSet<MaterialRequirments> MaterialRequirments { get; set; }
		public DbSet<Inventory> Inventory { get; set; }
		public DbSet<ItemType> ItemTypes { get; set; }
		public DbSet<LabourRequirments> LabourRequirments { get; set; }
		public DbSet<Labour> Labours { get; set; }
		public DbSet<Client> Clients { get; set; }
		public DbSet<Project> Projects { get; set; }
		//public DbSet<Province> Provinces { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Client>()
				.HasMany<Project>(c => c.Projects)
				.WithOne(f => f.Client)
				.HasForeignKey(f => f.ClientId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
