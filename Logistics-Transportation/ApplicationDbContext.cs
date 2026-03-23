using Logistics_Transportation.Configurations;
using Logistics_Transportation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Loader> Loaders { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<TripLoaders> TripLoaders { get; set; } = null!;
        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<LicenceCategories> LicenceCategories { get; set; } = null!;
        public DbSet<ActionLog> ActionLogs { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyConfiguration(new DriverConfiguration());
            modelBuilder.ApplyConfiguration(new LicenceCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new LoaderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new TripConfiguration());
            modelBuilder.ApplyConfiguration(new TripLoadersConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
