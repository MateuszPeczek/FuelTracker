using Commands.VehicleCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.VehicleHandlerUnitTests
{
    public class DeleteVehicleCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<DeleteVehicle> deleteVehicleValidator;
        protected readonly ICommandHandler<DeleteVehicle> deleteVehicleHandler;

        public DeleteVehicleCommandHandlerTests()
        {
            deleteVehicleValidator = A.Fake<ICommandValidator<DeleteVehicle>>();
            deleteVehicleHandler = new DeleteVehicleHandler(unitOfWork, deleteVehicleValidator);
        }

        [Fact]
        public void CommandValid_VehicleDeleted()
        {
            var modelId = InsertModelToDatabase();
            var vehicleId = InsertVehicleToDatabase(modelId);
            var deleteVehicleCommand = new DeleteVehicle(vehicleId);

            A.CallTo(() => deleteVehicleValidator.Validate(deleteVehicleCommand)).DoesNothing();

            deleteVehicleHandler.Handle(deleteVehicleCommand);

            context.SaveChanges();
            var result = context.Vehicle.FirstOrDefault(m => m.Id == deleteVehicleCommand.Id);

            A.CallTo(() => deleteVehicleValidator.Validate(deleteVehicleCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Null(result);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var modelId = InsertModelToDatabase();
            var vehicleId = InsertVehicleToDatabase(modelId);
            var deleteVehicleCommand = new DeleteVehicle(vehicleId);

            A.CallTo(() => deleteVehicleValidator.Validate(deleteVehicleCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => deleteVehicleHandler.Handle(deleteVehicleCommand));
            A.CallTo(() => deleteVehicleValidator.Validate(deleteVehicleCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.Vehicle.FirstOrDefault(m => m.Id == deleteVehicleCommand.Id);
            Assert.NotNull(result);
            Assert.Equal(result.Id, vehicleId);
            Assert.Equal(result.ModelNameId, modelId);
        }
    }
}
