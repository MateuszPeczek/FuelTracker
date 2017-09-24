using Commands.ManufacturerCommands;
using Common.Interfaces;
using CustomExceptions.Manufacturer;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Manufacturers
{
    public class DeleteManufacturerValidatorTests
    {
        private readonly ICommandValidator<DeleteManufacturer> deleteManufacturerValidator;

        public DeleteManufacturerValidatorTests()
        {
            deleteManufacturerValidator = new DeleteManufacturerValidator();
        }

        #region Setup

        private DeleteManufacturer GetValidDeleteManufacturerCommand()
        {
            return new DeleteManufacturer(Guid.NewGuid());
        }

        #endregion

        [Fact]
        public void EmptyGuidId_ThrowsInvalidManufacturerIdException()
        {
            var stubCommand = GetValidDeleteManufacturerCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidManufacturerIdException>(() => deleteManufacturerValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyExceptions()
        {
            var stubCommand = GetValidDeleteManufacturerCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => deleteManufacturerValidator.Validate(stubCommand));
        }
    }
}
