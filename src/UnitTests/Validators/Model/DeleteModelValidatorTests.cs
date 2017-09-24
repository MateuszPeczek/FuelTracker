using Commands.ModelCommands;
using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Model
{
    public class DeleteModelValidatorTests
    {
        private readonly ICommandValidator<DeleteModel> deleteModelValidator;

        public DeleteModelValidatorTests()
        {
            deleteModelValidator = new DeleteModelValidator();
        }

        #region Setup

        private DeleteModel GetValidDeleteModelCommand()
        {
            return new DeleteModel(Guid.NewGuid(), Guid.NewGuid());
        }

        #endregion

        [Fact]
        public void EmptyGuidIdValue_ThrowsInvalidModelIdException()
        {
            var stubCommand = GetValidDeleteModelCommand();
            stubCommand.Id = new Guid();


            Assert.Throws<InvalidModelIdException>(() => deleteModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyGuidManufacturerIdValue_ThrowsInvalidModelIdException()
        {
            var stubCommand = GetValidDeleteModelCommand();
            stubCommand.ManufacturerId = new Guid();


            Assert.Throws<InvalidManufacturerIdException>(() => deleteModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyExceptions()
        {
            var stubCommand = GetValidDeleteModelCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => deleteModelValidator.Validate(stubCommand));
        }
    }
}
