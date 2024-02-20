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
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffPosition> StaffPositions { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<DesignBid> DesignBids { get; set; }
        public DbSet<DesignBidStaff> DesignBidStaff { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            //Many to Many Intersection (Design Bid and Staff)
            modelBuilder.Entity<DesignBidStaff>()
            .HasKey(d => new { d.StaffID, d.DesignBidID});

            //Add a unique index to the City/Province
            modelBuilder.Entity<City>()
            .HasIndex(c => new { c.Name, c.ProvinceID })
            .IsUnique();

            //Add this so you don't get Cascade Delete
            modelBuilder.Entity<Province>()
                .HasMany<City>(d => d.Cities)
                .WithOne(p => p.Province)
                .HasForeignKey(p => p.ProvinceID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
				.HasMany<Project>(c => c.Projects)
				.WithOne(f => f.Client)
				.HasForeignKey(f => f.ClientId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
