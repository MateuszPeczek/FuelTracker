using Domain.FuelStatisticsDomain;
using Domain.UserDomain;
using Domain.VehicleDomain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.UserStore;
using System;

namespace Persistence
{
    public class ApplicationContext : IdentityDbContext<User, UserRole, Guid>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=fuelTracker.db");
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<ConsumptionReport> ConsumptionReport { get; set; }
        public DbSet<FuelSummary> FuelSummary { get; set; }
        public DbSet<Engine> Engine { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<ModelName> ModelName { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
    }
}
