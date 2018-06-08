﻿using Domain.VehicleDomain;
using FakeItEasy;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;

namespace UnitTests.CommandsHandlers
{
    public abstract class BaseHandlersUnitTests
    {
        protected ApplicationContext context;
        protected readonly IUnitOfWork unitOfWork;

        protected BaseHandlersUnitTests()
        {
            unitOfWork = A.Fake<IUnitOfWork>();
            context = new ApplicationContext(GetContextBuilderOptions());
            A.CallTo(() => unitOfWork.Context).Returns(context);
        }

        protected DbContextOptions<ApplicationContext> GetContextBuilderOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;
        }

        protected bool StringCollectionsEqual(IEnumerable<string> firstCollection, IEnumerable<string> secondCollection)
        {
            if (firstCollection.Count() != secondCollection.Count())
                return false;

            for (int i = 0; i < firstCollection.Count(); i++)
            {
                if (firstCollection.ElementAt(i) != secondCollection.ElementAt(i))
                    return false;
            }

            return true;
        }

        #region Engine

        protected const int expectedEngineCylinders = 4;
        protected const int expectedEngineDisplacement = 2000;
        protected const FuelType expectedEngineFuelType = FuelType.Diesel;
        protected const string expectedEngineName = "TestEngine";
        protected const int expectedEnginePower = 100;
        protected const int expectedEngineTorque = 200;
        
        protected Guid InsertEngineToDatabase()
        {
            var id = Guid.NewGuid();

            var newEngine = new Engine()
            {
                Cylinders = expectedEngineCylinders,
                Displacement = expectedEngineDisplacement,
                FuelType = expectedEngineFuelType,
                Id = id,
                Name = expectedEngineName,
                Power = expectedEnginePower,
                Torque = expectedEngineTorque
            };

            unitOfWork.Context.Engine.Add(newEngine);
            context.SaveChanges();
            
            return id;
        }

        #endregion

        #region Manufacturer

        protected const string expectedManufacturerName = "TestManufacturer";
        
        protected Guid InsertManufacturerToDatabase()
        {
            var id = Guid.NewGuid();

            var newManufacturer = new Manufacturer()
            {
                Id = id,
                Name = expectedManufacturerName
            };

            unitOfWork.Context.Manufacturer.Add(newManufacturer);
            context.SaveChanges();

            return id;
        }

        protected Guid InsertManufacturerWithModelNamesToDatabase()
        {
            var id = Guid.NewGuid();

            var newManufacturer = new Manufacturer()
            {
                Id = id,
                Name = expectedManufacturerName,
                ModelNames = new List<ModelName>()
                {
                   new ModelName()
                   {
                       Id = Guid.NewGuid(),
                       ManufacturerId = id,
                       Name = "TestName1"
                   },
                   new ModelName()
                   {
                       Id = Guid.NewGuid(),
                       ManufacturerId = id,
                       Name = "TestName2"
                   },
                   new ModelName()
                   {
                       Id = Guid.NewGuid(),
                       ManufacturerId = id,
                       Name = "TestName3"
                   },
                }
            };

            unitOfWork.Context.Manufacturer.Add(newManufacturer);
            context.SaveChanges();

            return id;
        }

        #endregion

        #region ModelName

        protected const string expectedModelName = "TestModel";

        protected Guid InsertModelToDatabase(Guid? manufacturerId = null)
        {
            var id = Guid.NewGuid();

            var newModel = new ModelName()
            {
                Id = id,
                Name = expectedModelName,
                ManufacturerId = manufacturerId ?? Guid.NewGuid()
            };

            unitOfWork.Context.ModelName.Add(newModel);
            context.SaveChanges();

            return id;
        }

        #endregion

        #region Vehicle

        protected const int expectedVehicleProductionYear = 2000;

        protected Guid InsertVehicleToDatabase(Guid? modelId = null, Guid? engineId = null)
        {
            var id = Guid.NewGuid();

            var newVehicle = new Vehicle()
            {
                Id = id,
                ModelNameId = modelId ?? Guid.NewGuid(),
                ProductionYear = expectedVehicleProductionYear,
                EngineId = engineId ?? Guid.NewGuid()
            };

            unitOfWork.Context.Vehicle.Add(newVehicle);
            context.SaveChanges();

            return id;
        }

        #endregion

    }
}
