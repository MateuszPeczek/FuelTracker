using Domain.VehicleDomain;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;

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

            var newEngine = new Domain.VehicleDomain.Engine()
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

        #endregion


    }
}
