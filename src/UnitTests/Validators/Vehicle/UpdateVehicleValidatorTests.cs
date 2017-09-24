using Commands.VehicleCommands;
using Common.Interfaces;
using CustomExceptions.Engine;
using CustomExceptions.Vehicle;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Vehicle
{
    public class UpdateVehicleValidatorTests
    {
        private readonly ICommandValidator<UpdateVehicle> updateVehicleValidator;

        public UpdateVehicleValidatorTests()
        {
            updateVehicleValidator = new UpdateVehicleValidator();
        }

        #region Setup

        private UpdateVehicle GetValidUpdateVehicleCommand()
        {
            return new UpdateVehicle(Guid.NewGuid(), 2000, Guid.NewGuid());
        }

        #endregion

        [Fact]
        public void EmptyGuidIdValue_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidUpdateVehicleCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidVehicleIdException>(() => updateVehicleValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyGuidEngineIdValue_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidUpdateVehicleCommand();
            stubCommand.EngineId = new Guid();

            Assert.Throws<InvalidEngineIdException>(() => updateVehicleValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyExceptions()
        {
            var stubCommand = GetValidUpdateVehicleCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => updateVehicleValidator.Validate(stubCommand));
        }
    }
}
