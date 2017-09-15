using Commands.EngineCommands;
using Common.Interfaces;
using CustomExceptions.Vehicle;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Engine
{
    public class UpdateEngineValidatorTests
    {
        private readonly ICommandValidator<UpdateEngine> updateEngineCommandValidator;

        public UpdateEngineValidatorTests()
        {
            updateEngineCommandValidator = new UpdateEngineValidaotr();
        }

        #region Setup

        private UpdateEngine GetValidUpdateEngineCommand()
        {
            return new UpdateEngine(Guid.NewGuid(), "TestEngine", 100,200,4,2000);
        }

        #endregion

        [Fact]
        private void EmptyGuidValue_TrowsInvalidEngineIdException()
        {
            var stubCommand = GetValidUpdateEngineCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidEngineIdException>(() => updateEngineCommandValidator.Validate(stubCommand));
        }

        [Fact]
        private void ValidCommand_NotThrowsAnyExceptions()
        {
            var stubCommand = GetValidUpdateEngineCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => updateEngineCommandValidator.Validate(stubCommand));
        }
    }
}
