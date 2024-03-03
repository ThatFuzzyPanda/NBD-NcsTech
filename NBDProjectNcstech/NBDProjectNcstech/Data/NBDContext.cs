using MedicalOffice.Models;
using Microsoft.EntityFrameworkCore;
using NBDProjectNcstech.Models;

namespace NBDProjectNcstech.Data
{
    public class NBDContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        //public NBDContext(DbContextOptions<NBDContext> options, IHttpContextAccessor httpContextAccessor)
        //    : base(options)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //    UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
        //    UserName = UserName ?? "Unknown";
        //}

        public NBDContext(DbContextOptions<NBDContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
                UserName ??= "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
        }
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
            .HasKey(d => new { d.StaffID, d.DesignBidID });

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
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                    
                }
                if (entry.Entity is IApproval Atrackable)
                {
                    var anow = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            Atrackable.UpdatedOn = anow;
                            Atrackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            Atrackable.CreatedOn = anow;
                            Atrackable.CreatedBy = UserName;
                            Atrackable.UpdatedOn = anow;
                            Atrackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
        public void ApproveEntity()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IApproval trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.ApprovedOn = now;
                            trackable.ApprovedBy = UserName;
                            break;
                    }
                }
            }
        }

        // Manual Auditing Method for Rejection
        public void RejectEntity()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IApproval trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.RejectedOn = now;
                            trackable.RejectedBy = UserName;
                            break;
                    }
                }
            }


        }

    }
}

