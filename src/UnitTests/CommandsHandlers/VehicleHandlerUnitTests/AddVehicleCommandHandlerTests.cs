using Commands.VehicleCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.VehicleHandlerUnitTests
{
    public class AddVehicleCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<AddVehicle> addVehicleValidator;
        protected readonly ICommandHandler<AddVehicle> addVehicleHandler;

        public AddVehicleCommandHandlerTests()
        {
            addVehicleValidator = A.Fake<ICommandValidator<AddVehicle>>();
            addVehicleHandler = new AddVehicleHandler(unitOfWork, addVehicleValidator);
        }

        [Fact]
        public void CommandValid_VehicleAdded()
        {
            var modelId = InsertModelToDatabase();
            var addVehicleCommand = new AddVehicle(modelId);
            
            A.CallTo(() => addVehicleValidator.Validate(addVehicleCommand)).DoesNothing();

            addVehicleHandler.Handle(addVehicleCommand);
            context.SaveChanges();

            var result = context.Vehicle.FirstOrDefault(v => v.Id == addVehicleCommand.Id);

            A.CallTo(() => addVehicleValidator.Validate(addVehicleCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(addVehicleCommand.Id, result.Id);
            Assert.Equal(addVehicleCommand.ModelNameId, result.ModelNameId);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var modelId = InsertModelToDatabase();
            var addVehicleCommand = new AddVehicle(modelId);

            A.CallTo(() => addVehicleValidator.Validate(addVehicleCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => addVehicleHandler.Handle(addVehicleCommand));

            context.SaveChanges();
            var result = context.Vehicle.FirstOrDefault(v => v.Id == addVehicleCommand.Id);

            A.CallTo(() => addVehicleValidator.Validate(addVehicleCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Null(result);
        }
    }
}
