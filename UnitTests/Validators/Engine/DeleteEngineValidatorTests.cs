using Commands.EngineCommands;
using Common.Interfaces;
using CustomExceptions.Vehicle;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Engine
{
    public class DeleteEngineValidatorTests
    {
        private readonly ICommandValidator<DeleteEngine> deleteEngineCommandValidator;

        public DeleteEngineValidatorTests()
        {
            deleteEngineCommandValidator = new DeleteEngineValidator();
        }

        #region Setup

        private DeleteEngine GetValidDeleteEngineCommand()
        {
            return new DeleteEngine(Guid.NewGuid());
        }

        #endregion

        [Fact]
        private void EmptyGuidValue_ThrowsInvalidEngineIdException()
        {
            var stubCommand = GetValidDeleteEngineCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidEngineIdException>(() => deleteEngineCommandValidator.Validate(stubCommand));
        }

        [Fact]
        private void ValidCommand_NotTrowsAnyExceptions()
        {
            var stubCommand = GetValidDeleteEngineCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => deleteEngineCommandValidator.Validate(stubCommand));
        }
    }
}
