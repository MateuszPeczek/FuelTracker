using Domain.FuelStatisticsDomain;
using Domain.UserDomain;
using Domain.VehicleDomain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence
{
    public class ApplicationContext : IdentityDbContext<User, UserRole, Guid>
    {
        public ApplicationContext()
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<ConsumptionReport> ConsumptionReport { get; set; }
        public DbSet<FuelSummary> FuelSummary { get; set; }
        public DbSet<Engine> Engine { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<ModelName> ModelName { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
