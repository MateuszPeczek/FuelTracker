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
                    new Manufacturer() { Id = new Guid("d0428eca-caaa-4ea7-9661-d5a4221fc7ff"), Name = "Alfa Romeo" },
                    new Manufacturer() { Id = new Guid("b33cb901-6da5-47b5-a2ad-3e821c1bfbdd"), Name = "BMW" },
                    new Manufacturer() { Id = new Guid("904b0afd-6bd1-4d25-81c1-e9b3cf65e2d8"), Name = "Dodge" },
                    new Manufacturer() { Id = new Guid("6783e8aa-9186-4a2c-81b5-58f622683239"), Name = "Ford" },
                    new Manufacturer() { Id = new Guid("cd7cf296-702a-4b55-931f-9b24a931c1a1"), Name = "Honda" },
                    new Manufacturer() { Id = new Guid("64810dcc-79f6-4cab-8891-625bcb50585f"), Name = "Merces" },
                    new Manufacturer() { Id = new Guid("10e8bdfb-66a2-41c9-b61b-c4de3037b874"), Name = "Volvo" }
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
                    new ModelName() { ManufacturerId = new Guid("d0428eca-caaa-4ea7-9661-d5a4221fc7ff"), Name = "Gulietta" },
                    new ModelName() { ManufacturerId = new Guid("d0428eca-caaa-4ea7-9661-d5a4221fc7ff"), Name = "Gulia" },
                    new ModelName() { ManufacturerId = new Guid("b33cb901-6da5-47b5-a2ad-3e821c1bfbdd"), Name = "3" },
                    new ModelName() { ManufacturerId = new Guid("b33cb901-6da5-47b5-a2ad-3e821c1bfbdd"), Name = "5" },
                    new ModelName() { ManufacturerId = new Guid("904b0afd-6bd1-4d25-81c1-e9b3cf65e2d8"), Name = "Viper" },
                    new ModelName() { ManufacturerId = new Guid("904b0afd-6bd1-4d25-81c1-e9b3cf65e2d8"), Name = "Charger" },
                    new ModelName() { ManufacturerId = new Guid("6783e8aa-9186-4a2c-81b5-58f622683239"), Name = "Fiesta" },
                    new ModelName() { ManufacturerId = new Guid("6783e8aa-9186-4a2c-81b5-58f622683239"), Name = "Focus" },
                    new ModelName() { ManufacturerId = new Guid("cd7cf296-702a-4b55-931f-9b24a931c1a1"), Name = "Accord" },
                    new ModelName() { ManufacturerId = new Guid("cd7cf296-702a-4b55-931f-9b24a931c1a1"), Name = "Civic" },
                    new ModelName() { ManufacturerId = new Guid("64810dcc-79f6-4cab-8891-625bcb50585f"), Name = "S" },
                    new ModelName() { ManufacturerId = new Guid("64810dcc-79f6-4cab-8891-625bcb50585f"), Name = "SLS" },
                    new ModelName() { ManufacturerId = new Guid("10e8bdfb-66a2-41c9-b61b-c4de3037b874"), Name = "V70" },
                    new ModelName() { ManufacturerId = new Guid("10e8bdfb-66a2-41c9-b61b-c4de3037b874"), Name = "S90" }
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
                    new Engine() {Id = new Guid("5b6a048a-ce00-48f9-815b-9e287efe2588"), Cylinders = 4, Displacement = 2000, FuelType = FuelType.Diesel, Name = "D4202T", Power = 136, Torque = 400 }
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
                    EngineId = new Guid("7ec813ae-3109-4fdc-ab05-2b6a31755b12"),
                    ModelNameId = new Guid("5b6a048a-ce00-48f9-815b-9e287efe2588"),
                    ProductionYear = 2008,
                    VehicleType = VehicleType.Car
                };

                context.Vehicle.AddRange(vehcile);
                context.SaveChanges();
            }
        }
    }
}
