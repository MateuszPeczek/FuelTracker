using Commands.ModelCommands;
using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Model
{
    public class UpdateModelValidatorTests
    {
        private readonly ICommandValidator<UpdateModel> updateModelValidator;

        public UpdateModelValidatorTests()
        {
            updateModelValidator = new UpdateModelValidator();
        }

        #region Setup

        private UpdateModel GetValidUpdateModelCommand()
        {
            return new UpdateModel(Guid.NewGuid(), Guid.NewGuid(), "Test");
        }

        #endregion  

        [Fact]
        public void EmptyGuidIdValue_ThrowsInvalidModelIdException()
        {
            var stubCommand = GetValidUpdateModelCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidModelIdException>(() => updateModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyGuidManufacturerIdValue_ThrowsInvalidManufactuterIdException()
        {
            var stubCommand = GetValidUpdateModelCommand();
            stubCommand.ManufacturerId = new Guid();

            Assert.Throws<InvalidManufacturerIdException>(() => updateModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void NullModelName_ThrowsEmptyModelException()
        {
            var stubCommand = GetValidUpdateModelCommand();
            stubCommand.Name = null;

            Assert.Throws<EmptyModelNameException>(() => updateModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyModelName_ThrowsEmptyModelException()
        {
            var stubCommand = GetValidUpdateModelCommand();
            stubCommand.Name = string.Empty;

            Assert.Throws<EmptyModelNameException>(() => updateModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void WhiteSpaceModelName_ThrowsEmptyModelException()
        {
            var stubCommand = GetValidUpdateModelCommand();
            stubCommand.Name = " ";

            Assert.Throws<EmptyModelNameException>(() => updateModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyException()
        {
            var stubCommand = GetValidUpdateModelCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => updateModelValidator.Validate(stubCommand));
        }
    }
}
