using Commands.VehicleCommands;
using Common.Interfaces;
using CustomExceptions.Vehicle;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.Vehicle
{
    public class DeleteVehicleValidatorTests
    {
        private readonly ICommandValidator<DeleteVehicle> deleteVehicleValidator;

        public DeleteVehicleValidatorTests()
        {
            deleteVehicleValidator = new DeleteVehicleValidator();
        }

        #region Setup

        private DeleteVehicle GetValidDeleteVehicleCommand()
        {
            return new DeleteVehicle(Guid.NewGuid());
        }

        #endregion

        [Fact]
        public void EmptyGuidIdValue_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidDeleteVehicleCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidVehicleIdException>(() => deleteVehicleValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnytExceptions()
        {
            var stubCommand = GetValidDeleteVehicleCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => deleteVehicleValidator.Validate(stubCommand));
        }
    }
}
