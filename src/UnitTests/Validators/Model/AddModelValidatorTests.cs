using Commands.ModelCommands;
using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Model
{
    public class AddModelValidatorTests
    {
        private readonly ICommandValidator<AddModelName> addModelValidator;

        public AddModelValidatorTests()
        {
            addModelValidator = new AddModelNameValidator();
        }

        #region Setup

        private AddModelName GetValidAddModelCommand()
        {
            return new AddModelName(Guid.NewGuid(), "Test");
        }

        #endregion  

        [Fact]
        public void EmptyGuidIdValue_ThrowsInvalidModelIdException()
        {
            var stubCommand = GetValidAddModelCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidModelIdException>(() => addModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyGuidModelIdValue_ThrowsInvalidModelIdException()
        {
            var stubCommand = GetValidAddModelCommand();
            stubCommand.ManufacturerId = new Guid();

            Assert.Throws<InvalidManufacturerIdException>(() => addModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void NullModelName_ThrowsEmptyModelException()
        {
            var stubCommand = GetValidAddModelCommand();
            stubCommand.Name = null;

            Assert.Throws<EmptyModelNameException>(() => addModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyModelName_ThrowsEmptyModelException()
        {
            var stubCommand = GetValidAddModelCommand();
            stubCommand.Name = string.Empty;

            Assert.Throws<EmptyModelNameException>(() => addModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void WhiteSpaceModelName_ThrowsEmptyModelException()
        {
            var stubCommand = GetValidAddModelCommand();
            stubCommand.Name = " ";

            Assert.Throws<EmptyModelNameException>(() => addModelValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyException()
        {
            var stubCommand = GetValidAddModelCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => addModelValidator.Validate(stubCommand));
        }
    }
}
