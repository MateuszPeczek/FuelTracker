using Domain.VehicleDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public static class DataSeed
    {
        public static void SeedData(this IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var context = (ApplicationContext)serviceProvider.GetRequiredService(typeof(ApplicationContext));

            SeedManufacturers(context);
            SeedModels(context);
            SeedEngines(context);
            SeedVehicle(context);
            
        }

        private static void SeedManufacturers(ApplicationContext context)
        {

            if (!context.Manufacturer.Any())
            {
                var manufacturers = new List<Manufacturer>()
                {
                    new Manufacturer() { Name = "Alfa Romeo" },
                    new Manufacturer() { Name = "BMW" },
                    new Manufacturer() { Name = "Dodge" },
                    new Manufacturer() { Name = "Ford" },
                    new Manufacturer() { Name = "Honda" },
                    new Manufacturer() { Name = "Merces" },
                    new Manufacturer() { Name = "Volvo" }
                };

                context.Manufacturer.AddRange(manufacturers);
                context.SaveChanges();
            }
        }

        private static void SeedModels(ApplicationContext context)
        {
            if (!context.ModelName.Any())
            {
                var models = new List<ModelName>()
                {
                    new ModelName() { ManufacturerId = 1, Name = "Gulietta" },
                    new ModelName() { ManufacturerId = 1, Name = "Gulia" },
                    new ModelName() { ManufacturerId = 2, Name = "3" },
                    new ModelName() { ManufacturerId = 2, Name = "5" },
                    new ModelName() { ManufacturerId = 3, Name = "Viper" },
                    new ModelName() { ManufacturerId = 3, Name = "Charger" },
                    new ModelName() { ManufacturerId = 4, Name = "Fiesta" },
                    new ModelName() { ManufacturerId = 4, Name = "Focus" },
                    new ModelName() { ManufacturerId = 5, Name = "Accord" },
                    new ModelName() { ManufacturerId = 5, Name = "Civic" },
                    new ModelName() { ManufacturerId = 6, Name = "S" },
                    new ModelName() { ManufacturerId = 6, Name = "SLS" },
                    new ModelName() { ManufacturerId = 7, Name = "V70" },
                    new ModelName() { ManufacturerId = 7, Name = "S90" }
                };

                context.ModelName.AddRange(models);
                context.SaveChanges();
            }
        }

        private static void SeedEngines(ApplicationContext context)
        {
            if (!context.Engine.Any())
            {
                var engines = new List<Engine>()
                {
                    new Engine() { Cylinders = 4, Displacement = 2000, FuelType = FuelType.Diesel, Name = "D4202T", Power = 136, Torque = 400 }
                };

                context.Engine.AddRange(engines);
                context.SaveChanges();
            }
        }

        private static void SeedVehicle(ApplicationContext context)
        {
            if (!context.Vehicle.Any())
            {
                var vehcile = new Vehicle()
                {
                    EngineId = 1,
                    Guid = Guid.NewGuid(),
                    ModelNameId = 13,
                    ProductionYear = 2008,
                    VehicleType = VehicleType.Car
                };

                context.Vehicle.AddRange(vehcile);
                context.SaveChanges();
            }
        }
    }
}
