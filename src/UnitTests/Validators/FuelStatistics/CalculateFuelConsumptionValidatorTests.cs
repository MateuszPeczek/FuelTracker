using Commands.FuelStatisticsCommands;
using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using CustomExceptions.User;
using CustomExceptions.Vehicle;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.FuelStatistics
{
    public class CalculateFuelConsumptionValidatorTests
    {
        private readonly ICommandValidator<CalculateFuelConsumption> calculateFuelConsumptionValidator;

        public CalculateFuelConsumptionValidatorTests()
        {
            calculateFuelConsumptionValidator = new CalculateFuelConsumptionValidator();
        }

        #region Setup

        public CalculateFuelConsumption GetValidCalculateFuelConsumptionCommand()
        {
            return new CalculateFuelConsumption(Guid.NewGuid(), Guid.NewGuid(), 100, 5, 4);
        }

        #endregion

        [Fact]
        public void EmptyIdGuidValue_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidCalculateFuelConsumptionCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidCalculateFuelConsumptionIdException>(() => calculateFuelConsumptionValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyUserGuidValue_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidCalculateFuelConsumptionCommand();
            stubCommand.UserId = new Guid();

            Assert.Throws<InvalidUserIdException>(() => calculateFuelConsumptionValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyVehicleGuidValue_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidCalculateFuelConsumptionCommand();
            stubCommand.VehicleId = new Guid();

            Assert.Throws<InvalidVehicleIdException>(() => calculateFuelConsumptionValidator.Validate(stubCommand));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DistanceLessThanZero_ThrowsInvalidDistanceException(int distance)
        {
            var stubCommand = GetValidCalculateFuelConsumptionCommand();
            stubCommand.Distance = distance;

            Assert.Throws<InvalidDistanceException>(() => calculateFuelConsumptionValidator.Validate(stubCommand));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void FuelBurnedLessThanZero_ThrowsInvalidFuelBurnedException(float fuelBurned)
        {
            var stubCommand = GetValidCalculateFuelConsumptionCommand();
            stubCommand.FuelBurned = fuelBurned;

            Assert.Throws<InvalidFuelBurnedException>(() => calculateFuelConsumptionValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyExceptions()
        {
            var stubCommand = GetValidCalculateFuelConsumptionCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => calculateFuelConsumptionValidator.Validate(stubCommand));
        }
    }
}
