using Commands.VehicleCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.VehicleHandlerUnitTests
{
    public class UpdateVehicleCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<UpdateVehicle> updateVehicleValidator;
        protected readonly ICommandHandler<UpdateVehicle> updateVehicleHandler;

        public UpdateVehicleCommandHandlerTests()
        {
            updateVehicleValidator = A.Fake<ICommandValidator<UpdateVehicle>>();
            updateVehicleHandler = new UpdateVehicleHandler(unitOfWork, updateVehicleValidator);
        }

        [Fact]
        public void CommandValid_EngineUpdated()
        {
            var vehicleId = InsertVehicleToDatabase();
            var engineId = InsertEngineToDatabase();
            var updateVehicleCommand = new UpdateVehicle(vehicleId, 2010, engineId);

            A.CallTo(() => updateVehicleValidator.Validate(updateVehicleCommand)).DoesNothing();

            updateVehicleHandler.Handle(updateVehicleCommand);

            context.SaveChanges();
            var result = context.Vehicle.FirstOrDefault(v => v.Id == updateVehicleCommand.Id);

            A.CallTo(() => updateVehicleValidator.Validate(updateVehicleCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(result.Id, updateVehicleCommand.Id);
            Assert.Equal(result.ProductionYear, updateVehicleCommand.ProductionYear);
            Assert.Equal(result.EngineId, updateVehicleCommand.EngineId);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var previousEngineGuid = Guid.NewGuid();
            var engineId = InsertEngineToDatabase();
            var vehicleId = InsertVehicleToDatabase(engineId: previousEngineGuid);
            var updateVehicleCommand = new UpdateVehicle(vehicleId, 2010, engineId);

            A.CallTo(() => updateVehicleValidator.Validate(updateVehicleCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => updateVehicleHandler.Handle(updateVehicleCommand));
            A.CallTo(() => updateVehicleValidator.Validate(updateVehicleCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.Vehicle.FirstOrDefault(v => v.Id == updateVehicleCommand.Id);
            Assert.NotNull(result);
            Assert.Equal(vehicleId, result.Id);
            Assert.Equal(expectedVehicleProductionYear, result.ProductionYear);
            Assert.Equal(previousEngineGuid, result.EngineId);
        }
    }
}
