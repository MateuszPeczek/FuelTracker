using Commands.VehicleCommands;
using Common.Interfaces;
using CustomExceptions.Model;
using CustomExceptions.Vehicle;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Vehicle
{
    public class AddVehicleValidatorTests
    {
        private readonly ICommandValidator<AddVehicle> addVehicleValidator;

        public AddVehicleValidatorTests()
        {
            addVehicleValidator = new AddVehicleValidator();
        }

        #region Setup

        private AddVehicle GetValidAddVehicleCommand()
        {
            return new AddVehicle(Guid.NewGuid());
        }

        #endregion

        [Fact]
        public void EmptyGuidIdValue_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidAddVehicleCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidVehicleIdException>(() => addVehicleValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyGuidModelIdValue_ThrowsInvalidModelIdException()
        {
            var stubCommand = GetValidAddVehicleCommand();
            stubCommand.ModelNameId = new Guid();

            Assert.Throws<InvalidModelIdException>(() => addVehicleValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyExceptions()
        {
            var stubCommand = GetValidAddVehicleCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => addVehicleValidator.Validate(stubCommand));
        }
    }
}
