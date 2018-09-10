using Domain.VehicleDomain;
using FakeItEasy;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using Domain.UserDomain;

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
            context.Database.EnsureCreated();
            A.CallTo(() => unitOfWork.Context).Returns(context);
        }

        protected DbContextOptions<ApplicationContext> GetContextBuilderOptions()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>()
                            .UseSqlite("DataSource=:memory:")
                            .Options;

            return builder;
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
            manufacturerId = manufacturerId ?? Guid.NewGuid();

            var newModel = new ModelName()
            {
                Id = id,
                Name = expectedModelName,
                ManufacturerId = manufacturerId.Value,
                Manufacturer = new Manufacturer() { Id = manufacturerId.Value }
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
            var manufacturerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            modelId = modelId ?? Guid.NewGuid();
            engineId = engineId ?? Guid.NewGuid();


            var newVehicle = new Vehicle()
            {
                Id = id,
                ModelNameId = modelId.Value,
                ModelName = new ModelName()
                {
                        Id = modelId.Value,
                        Manufacturer = new Manufacturer()
                        {
                            Id = manufacturerId
                        },
                        ManufacturerId = manufacturerId
                },
                ProductionYear = expectedVehicleProductionYear,
                EngineId = engineId.Value,
                Engine = new Engine() { Id = engineId.Value},
                UserId = userId,
                User = new User() { Id = userId }
            };

            unitOfWork.Context.Vehicle.Add(newVehicle);
            context.SaveChanges();

            return id;
        }

        #endregion

        #region User

        protected const string userEmail = "test@email.com";
        protected const string userName = "test";
        protected const string password = "password";
        protected const string firstName = "Name";

        protected Guid InsertUserToDatabase(Guid? userId = null)
        {
            var id = userId ?? Guid.NewGuid();

            var newUser = new User()
            {
                Id = id,
                Email = userEmail,
                UserName = userName,
                PasswordHash = password.GetHashCode().ToString(),
                FirstName = firstName
            };

            unitOfWork.Context.User.Add(newUser);
            context.SaveChanges();

            return id;
        }

        #endregion   
    }
}
