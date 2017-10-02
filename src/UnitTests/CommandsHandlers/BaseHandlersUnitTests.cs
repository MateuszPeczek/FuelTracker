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

        protected const int expectedCylinders = 4;
        protected const int expectedDisplacement = 2000;
        protected const FuelType expectedFuelType = FuelType.Diesel;
        protected const string expectedName = "Test";
        protected const int expectedPower = 100;
        protected const int expectedTorque = 200;

        protected Guid InsertEngineToDatabase()
        {
            var id = Guid.NewGuid();

            var newEngine = new Domain.VehicleDomain.Engine()
            {
                Cylinders = expectedCylinders,
                Displacement = expectedDisplacement,
                FuelType = expectedFuelType,
                Id = id,
                Name = expectedName,
                Power = expectedPower,
                Torque = expectedTorque
            };

            unitOfWork.Context.Engine.Add(newEngine);

            context.SaveChanges();

            return id;
        }
    }
}
