﻿using Commands.EngineCommands;
using Common.Interfaces;
using FakeItEasy;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.CommandsHandlers.EngineHandler
{
    public class UpdateEngineCommandHandlerTests : BaseHandlersUnitTests
    {
        protected readonly ICommandValidator<UpdateEngine> updateEngineValidator;
        protected readonly ICommandHandler<UpdateEngine> updateEngineHandler;
        
        public UpdateEngineCommandHandlerTests()
        {
            updateEngineValidator = A.Fake<ICommandValidator<UpdateEngine>>();
            updateEngineHandler = new UpdateEngineHandler(unitOfWork, updateEngineValidator);
        }

        [Fact]
        public void CommandValid_EngineUpdated()
        {
            var engineId = InsertEngineToDatabase();
            var updateEngineCommand = new UpdateEngine(engineId, "Updated", power: 150, torque: 300, cylinders: 6, displacement: 3000);

            A.CallTo(() => updateEngineValidator.Validate(updateEngineCommand)).DoesNothing();

            updateEngineHandler.Handle(updateEngineCommand);

            context.SaveChanges();
            var result = context.Engine.FirstOrDefault(e => e.Id == updateEngineCommand.Id);

            A.CallTo(() => updateEngineValidator.Validate(updateEngineCommand)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(result);
            Assert.Equal(result.Id, updateEngineCommand.Id);
            Assert.Equal(result.Name, updateEngineCommand.Name);
            Assert.Equal(result.Power, updateEngineCommand.Power);
            Assert.Equal(result.Torque, updateEngineCommand.Torque);
            Assert.Equal(result.Cylinders, updateEngineCommand.Cylinders);
            Assert.Equal(result.Displacement, updateEngineCommand.Displacement);
        }

        [Fact]
        public void ValidatorThrowsException_HandlerThrowsException()
        {
            var engineId = InsertEngineToDatabase();
            var updateEngineCommand = new UpdateEngine(engineId, "Updated", power: 150, torque: 300, cylinders: 6, displacement: 3000);

            A.CallTo(() => updateEngineValidator.Validate(updateEngineCommand)).Throws<Exception>();

            Assert.ThrowsAny<Exception>(() => updateEngineHandler.Handle(updateEngineCommand));
            A.CallTo(() => updateEngineValidator.Validate(updateEngineCommand)).MustHaveHappened(Repeated.Exactly.Once);

            context.SaveChanges();
            var result = context.Engine.FirstOrDefault(e => e.Id == updateEngineCommand.Id);
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
