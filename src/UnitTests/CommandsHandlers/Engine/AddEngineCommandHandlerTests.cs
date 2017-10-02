using Commands.EngineCommands;
using Common.Interfaces;
using Domain.VehicleDomain;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.Engine
{
    public class AddEngineCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<AddEngine> addEngineValidator;
        protected readonly ICommandHandler<AddEngine> addEngineHandler;

        public AddEngineCommandHandlerTests()
        {
            addEngineValidator = A.Fake<ICommandValidator<AddEngine>>();
            addEngineHandler = new AddEngineHandler(unitOfWork, addEngineValidator);
        }

        [Fact]
        public void CommandValid_EngineAdded()
        {
            var addEngineCommand = new AddEngine("Test", 10,10,4,10,FuelType.Diesel);

            A.CallTo(() => addEngineValidator.Validate(addEngineCommand)).DoesNothing();

            addEngineHandler.Handle(addEngineCommand);

            context.SaveChanges();
            var result = context.Engine.FirstOrDefault(e => e.Id == addEngineCommand.Id);
            
            A.CallTo(() => addEngineValidator.Validate(addEngineCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(result.Id, addEngineCommand.Id);
            Assert.Equal(result.Name, addEngineCommand.Name);
            Assert.Equal(result.Power, addEngineCommand.Power);
            Assert.Equal(result.Torque, addEngineCommand.Torque);
            Assert.Equal(result.Cylinders, addEngineCommand.Cylinders);
            Assert.Equal(result.Displacement, addEngineCommand.Displacement);
            Assert.Equal(result.FuelType, addEngineCommand.FuelType);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var addEngineCommand = new AddEngine("Test", 10, 10, 4, 10, FuelType.Diesel);

            A.CallTo(() => addEngineValidator.Validate(addEngineCommand)).Throws<Exception>();
            
            Assert.ThrowsAny<Exception>(() => addEngineHandler.Handle(addEngineCommand));
            A.CallTo(() => addEngineValidator.Validate(addEngineCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.Engine.FirstOrDefault(e => e.Id == addEngineCommand.Id);
            Assert.Null(result);
        }
    }
}
