using Commands.ManudaturerCommands;
using Common.Interfaces;
using CustomExceptions.Manufacturer;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Manufacturers
{
    public class AddManufacturerValidatorTests
    {
        private readonly ICommandValidator<AddManufacturer> addManufacturerValidator;

        public AddManufacturerValidatorTests()
        {
            addManufacturerValidator = new AddManufacturerValidator();
        }

        #region Setup

        private AddManufacturer GetValidAddManufacturerCommand()
        {
            return new AddManufacturer("Test");
        }

        #endregion

        [Fact]
        public void EmptyGuidIdValue_ThrowsInvalidManufacturerIdException()
        {
            var stubCommand = GetValidAddManufacturerCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidManufacturerIdException>(() => addManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void NullManufacturerName_ThrowsEmptyManufacturerException()
        {
            var stubCommand = GetValidAddManufacturerCommand();
            stubCommand.Name = null;

            Assert.Throws<EmptyManufacturerNameException>(() => addManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyManufacturerName_ThrowsEmptyManufacturerException()
        {
            var stubCommand = GetValidAddManufacturerCommand();
            stubCommand.Name = string.Empty;

            Assert.Throws<EmptyManufacturerNameException>(() => addManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void WhiteSpaceManufacturerName_ThrowsEmptyManufacturerException()
        {
            var stubCommand = GetValidAddManufacturerCommand();
            stubCommand.Name = " ";

            Assert.Throws<EmptyManufacturerNameException>(() => addManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyException()
        {
            var stubCommand = GetValidAddManufacturerCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => addManufacturerValidator.Validate(stubCommand));
        }
    }
}
