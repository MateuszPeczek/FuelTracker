using Commands.ManufacturerCommands;
using Common.Interfaces;
using CustomExceptions.Manufacturer;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Manufacturers
{
    public class UpdateManufacturerValidatorTests
    {
        private readonly ICommandValidator<UpdateManufacturer> updateManufacturerValidator;

        public UpdateManufacturerValidatorTests()
        {
            updateManufacturerValidator = new UpdateManufacturerValidator();
        }

        #region Setup

        private UpdateManufacturer GetValidUpdateManufacturerCommand()
        {
            return new UpdateManufacturer(Guid.NewGuid(), "Test");
        }

        #endregion

        [Fact]
        public void EmptyGuidIdValue_ThrowsInvalidManufactuterIdException()
        {
            var stubCommand = GetValidUpdateManufacturerCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidManufacturerIdException>(() => updateManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void NullManufacturerName_ThrowsEmptyManufacturerException()
        {
            var stubCommand = GetValidUpdateManufacturerCommand();
            stubCommand.Name = null;

            Assert.Throws<EmptyManufacturerNameException>(() => updateManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyManufacturerName_ThrowsEmptyManufacturerException()
        {
            var stubCommand = GetValidUpdateManufacturerCommand();
            stubCommand.Name = string.Empty;

            Assert.Throws<EmptyManufacturerNameException>(() => updateManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void WhiteSpaceManufacturerName_ThrowsEmptyManufacturerException()
        {
            var stubCommand = GetValidUpdateManufacturerCommand();
            stubCommand.Name = " ";

            Assert.Throws<EmptyManufacturerNameException>(() => updateManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyException()
        {
            var stubCommand = GetValidUpdateManufacturerCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => updateManufacturerValidator.Validate(stubCommand));
        }
    }
}
