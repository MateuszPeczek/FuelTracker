using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Persistence;
using Domain.Common;
using Domain.VehicleDomain;

namespace Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.FuelStatisticsDomain.ConsumptionReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<float>("Distance");

                    b.Property<float>("FuelBurned");

                    b.Property<float>("FuelEfficiency");

                    b.Property<decimal>("PricePerUnit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("Units");

                    b.Property<long>("VehicleId")
                        .HasColumnName("Vehicle");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("ConsumptionReport");
                });

            modelBuilder.Entity("Domain.FuelStatisticsDomain.FuelSummary", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("AverageConsumption");

                    b.Property<decimal>("DistanceDriven");

                    b.Property<decimal>("FuelBurned");

                    b.Property<decimal>("MoneySpent");

                    b.Property<long>("ReportsNumber");

                    b.Property<long>("VehicleId");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("FuelSummary");
                });

            modelBuilder.Entity("Domain.UserDomain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Domain.UserDomain.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Domain.VehicleDomain.Engine", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cylinders");

                    b.Property<float>("Displacement");

                    b.Property<int>("FuelType");

                    b.Property<string>("Name")
                        .HasMaxLength(15);

                    b.Property<int>("Power");

                    b.Property<int>("Torque");

                    b.HasKey("Id");

                    b.ToTable("Engine");
                });

            modelBuilder.Entity("Domain.VehicleDomain.Manufacturer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Manufacturer");
                });

            modelBuilder.Entity("Domain.VehicleDomain.ModelName", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ManufacturerId");

                    b.Property<string>("Name")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("ModelName");
                });

            modelBuilder.Entity("Domain.VehicleDomain.Vehicle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("EngineId");

                    b.Property<Guid>("Guid");

                    b.Property<long>("ModelNameId");

                    b.Property<int?>("ProductionYear");

                    b.Property<int?>("VehicleType");

                    b.HasKey("Id");

                    b.HasIndex("EngineId");

                    b.HasIndex("ModelNameId");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Domain.FuelStatisticsDomain.ConsumptionReport", b =>
                {
                    b.HasOne("Domain.VehicleDomain.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.FuelStatisticsDomain.FuelSummary", b =>
                {
                    b.HasOne("Domain.VehicleDomain.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.VehicleDomain.ModelName", b =>
                {
                    b.HasOne("Domain.VehicleDomain.Manufacturer", "Manufacturer")
                        .WithMany("ModelName")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.VehicleDomain.Vehicle", b =>
                {
                    b.HasOne("Domain.VehicleDomain.Engine", "Engine")
                        .WithMany("Vehicle")
                        .HasForeignKey("EngineId");

                    b.HasOne("Domain.VehicleDomain.ModelName", "ModelName")
                        .WithMany("Vehicle")
                        .HasForeignKey("ModelNameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("Domain.UserDomain.UserRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("Domain.UserDomain.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("Domain.UserDomain.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<long>", b =>
                {
                    b.HasOne("Domain.UserDomain.UserRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.UserDomain.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
