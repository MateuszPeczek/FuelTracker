﻿using Domain.VehicleDomain;
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
                    new ModelName() { Id = new Guid("b9384912-b8c1-4326-9d92-3092454812e1"), ManufacturerId = new Guid("d0428eca-caaa-4ea7-9661-d5a4221fc7ff"), Name = "Gulietta" },
                    new ModelName() { Id = new Guid("e429a333-493c-49c7-91b6-f8963b0f1c9a"), ManufacturerId = new Guid("d0428eca-caaa-4ea7-9661-d5a4221fc7ff"), Name = "Gulia" },
                    new ModelName() { Id = new Guid("49e7fa7d-d3f2-4fa0-8d6b-9dcf776a284b"), ManufacturerId = new Guid("b33cb901-6da5-47b5-a2ad-3e821c1bfbdd"), Name = "3" },
                    new ModelName() { Id = new Guid("300fb65a-100e-45e9-9b06-c3399a44c5f9"), ManufacturerId = new Guid("b33cb901-6da5-47b5-a2ad-3e821c1bfbdd"), Name = "5" },
                    new ModelName() { Id = new Guid("42b17b57-1ad9-4334-bca2-9d17f153a8cc"), ManufacturerId = new Guid("904b0afd-6bd1-4d25-81c1-e9b3cf65e2d8"), Name = "Viper" },
                    new ModelName() { Id = new Guid("d09cd51e-10dd-4967-acb1-d7a1a3f6adda"), ManufacturerId = new Guid("904b0afd-6bd1-4d25-81c1-e9b3cf65e2d8"), Name = "Charger" },
                    new ModelName() { Id = new Guid("6539b992-849c-4eea-a3db-8895b1ef6d0c"), ManufacturerId = new Guid("6783e8aa-9186-4a2c-81b5-58f622683239"), Name = "Fiesta" },
                    new ModelName() { Id = new Guid("a8dbc200-99c8-42c1-959c-fc4f45d8f65b"), ManufacturerId = new Guid("6783e8aa-9186-4a2c-81b5-58f622683239"), Name = "Focus" },
                    new ModelName() { Id = new Guid("ef467399-4f5a-45d5-8624-4a65d8a45784"), ManufacturerId = new Guid("cd7cf296-702a-4b55-931f-9b24a931c1a1"), Name = "Accord" },
                    new ModelName() { Id = new Guid("ff413a07-ac05-4545-8a04-ca17bbbd2ed4"), ManufacturerId = new Guid("cd7cf296-702a-4b55-931f-9b24a931c1a1"), Name = "Civic" },
                    new ModelName() { Id = new Guid("a0184a49-7c0c-4c9c-801d-bb11b476aace"), ManufacturerId = new Guid("64810dcc-79f6-4cab-8891-625bcb50585f"), Name = "S" },
                    new ModelName() { Id = new Guid("8ada07b7-512f-4d1c-a090-e4664a9c1b84"), ManufacturerId = new Guid("64810dcc-79f6-4cab-8891-625bcb50585f"), Name = "SLS" },
                    new ModelName() { Id = new Guid("f31b62aa-22fb-49dc-8fdf-c9576a657a3b"), ManufacturerId = new Guid("10e8bdfb-66a2-41c9-b61b-c4de3037b874"), Name = "V70" },
                    new ModelName() { Id = new Guid("cd62002a-b3a2-4464-b394-a5e8c993caec"), ManufacturerId = new Guid("10e8bdfb-66a2-41c9-b61b-c4de3037b874"), Name = "S90" }
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
                    new Engine() {Id = new Guid("5b6a048a-ce00-48f9-815b-9e287efe2588"), Cylinders = 4, Displacement = 2000, FuelType = FuelType.Diesel, Name = "D4202T", Power = 136, Torque = 400 },
                    new Engine() {Id = new Guid("3d8579fd-2f30-4eae-8e1f-9e24927cda24"), Cylinders = 4, Displacement = 2000, FuelType = FuelType.Petrol, Name = "T5", Power = 254, Torque = 350 },
                    new Engine() {Id = new Guid("35a66ff7-5bb3-436b-9202-a6f7e023e5a1"), Cylinders = 6, Displacement = 2979, FuelType = FuelType.Diesel, Name = "M54 B30", Power = 130, Torque = 350 },
                };

                context.Engine.AddRange(engines);
                context.SaveChanges();
            }
        }

        private static void SeedVehicle(ApplicationContext context)
        {
            if (!context.Vehicle.Any())
            {
                var vehciles = new List<Vehicle>()
                {
                    new Vehicle
                    {
                        Id = new Guid("d51b848f-c1b8-4a74-afb5-6730dd2554f5"),
                        EngineId = new Guid("5b6a048a-ce00-48f9-815b-9e287efe2588"),
                        ModelNameId = new Guid("f31b62aa-22fb-49dc-8fdf-c9576a657a3b"),
                        ProductionYear = 2008,
                        VehicleType = VehicleType.Car
                    },
                    new Vehicle
                    {
                        Id = new Guid("c4b31e8d-1788-4364-ae28-b0a5ec0e619e"),
                        EngineId = new Guid("3d8579fd-2f30-4eae-8e1f-9e24927cda24"),
                        ModelNameId = new Guid("cd62002a-b3a2-4464-b394-a5e8c993caec"),
                        ProductionYear = 2016,
                        VehicleType = VehicleType.Car
                    },
                    new Vehicle
                    {
                        Id = new Guid("091dbb3f-2c6c-4f3c-b8eb-99af897a1f10"),
                        EngineId = new Guid("35a66ff7-5bb3-436b-9202-a6f7e023e5a1"),
                        ModelNameId = new Guid("49e7fa7d-d3f2-4fa0-8d6b-9dcf776a284b"),
                        ProductionYear = 2003,
                        VehicleType = VehicleType.Car
                    },
                };

                context.Vehicle.AddRange(vehciles);
                context.SaveChanges();
            }
        }
    }
}