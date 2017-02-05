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
    [Migration("20170205205943_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.FuelStatisticsDomain.ConsumptionReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ConsumptionReportID");

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
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FuelSummaryID");

                    b.Property<float>("AverageConsumption");

                    b.Property<decimal>("DistanceDriven");

                    b.Property<decimal>("FuelBurned");

                    b.Property<decimal>("MoneySpent");

                    b.Property<long>("ReportsNumber");

                    b.Property<long>("VehicleId")
                        .HasColumnName("Vehicle");

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
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EngineID");

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

            modelBuilder.Entity("Domain.VehicleDomain.Vehicle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("VahicleID");

                    b.Property<long>("EngineID");

                    b.Property<int>("ProductionYear");

                    b.Property<int>("Type");

                    b.Property<long>("UserId");

                    b.Property<long>("VehicleManufacturerID");

                    b.Property<long>("VehicleModelID");

                    b.HasKey("Id");

                    b.HasIndex("EngineID");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleManufacturerID");

                    b.HasIndex("VehicleModelID");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("Domain.VehicleDomain.VehicleManufacturer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("VehicleManufacturerID");

                    b.Property<string>("Name")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("VehicleManufacturer");
                });

            modelBuilder.Entity("Domain.VehicleDomain.VehicleModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("VehicleModelID");

                    b.Property<string>("Name")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("VehicleModel");
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

            modelBuilder.Entity("Domain.VehicleDomain.Vehicle", b =>
                {
                    b.HasOne("Domain.VehicleDomain.Engine", "Engine")
                        .WithMany()
                        .HasForeignKey("EngineID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.UserDomain.User", "User")
                        .WithMany("Vehicles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.VehicleDomain.VehicleManufacturer", "VehicleManufacturer")
                        .WithMany()
                        .HasForeignKey("VehicleManufacturerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.VehicleDomain.VehicleModel", "VehicleModel")
                        .WithMany()
                        .HasForeignKey("VehicleModelID")
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
