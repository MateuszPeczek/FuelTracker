using Commands.EngineCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.Engine
{
    public class DeleteEngineCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<DeleteEngine> deleteEngineValidator;
        protected readonly ICommandHandler<DeleteEngine> deleteEngineHandler;

        public DeleteEngineCommandHandlerTests()
        {
            deleteEngineValidator = A.Fake<ICommandValidator<DeleteEngine>>();
            deleteEngineHandler = new DeleteEngineHandler(unitOfWork, deleteEngineValidator);
        }

        [Fact]
        public void CommandValid_EngineDeleted()
        {
            var engineId = InsertEngineToDatabase();
            var deleteEngineCommand = new DeleteEngine(engineId);

            A.CallTo(() => deleteEngineValidator.Validate(deleteEngineCommand)).DoesNothing();

            deleteEngineHandler.Handle(deleteEngineCommand);
            context.SaveChanges();

            var result = context.Engine.FirstOrDefault(e => e.Id == engineId);

            A.CallTo(() => deleteEngineValidator.Validate(deleteEngineCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Null(result);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var engineId = InsertEngineToDatabase();
            var deleteEngineCommand = new DeleteEngine(engineId);

            A.CallTo(() => deleteEngineValidator.Validate(deleteEngineCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => deleteEngineHandler.Handle(deleteEngineCommand));
            A.CallTo(() => deleteEngineValidator.Validate(deleteEngineCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.Engine.FirstOrDefault(e => e.Id == engineId);
            
            Assert.NotNull(result);
            Assert.Equal(engineId, result.Id);
            Assert.Equal(expectedEngineCylinders, result.Cylinders);
            Assert.Equal(expectedEngineDisplacement, result.Displacement);
            Assert.Equal(expectedEngineFuelType, result.FuelType);
            Assert.Equal(expectedEngineName, result.Name);
            Assert.Equal(expectedEnginePower, result.Power);
            Assert.Equal(expectedEngineTorque, result.Torque);
        }
    }
}
