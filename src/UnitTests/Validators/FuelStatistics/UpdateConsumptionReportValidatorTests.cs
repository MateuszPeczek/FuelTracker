using Commands.FuelStatisticsCommands;
using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using CustomExceptions.Vehicle;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.FuelStatistics
{
    public class UpdateConsumptionReportValidatorTests
    {
        private readonly ICommandValidator<UpdateConsumptionReport> updateConsumptionReportValidator;

        public UpdateConsumptionReportValidatorTests()
        {
            updateConsumptionReportValidator = new UpdateConsumptionReportValidator();
        }

        #region Setup

        public UpdateConsumptionReport GetValidUpdateConsumptionReportCommand()
        {
            return new UpdateConsumptionReport(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100, 5, 4);
        }

        #endregion

        [Fact]
        public void EmptyIdGuidValue_ThrowsInvalidCalculateFuelConsumptionIdException()
        {
            var stubCommand = GetValidUpdateConsumptionReportCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidConsumptionReportIdException>(() => updateConsumptionReportValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyVehicleGuidValue_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidUpdateConsumptionReportCommand();
            stubCommand.VehicleId = new Guid();

            Assert.Throws<InvalidVehicleIdException>(() => updateConsumptionReportValidator.Validate(stubCommand));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DistanceLessThanZero_ThrowsInvalidDistanceException(int distance)
        {
            var stubCommand = GetValidUpdateConsumptionReportCommand();
            stubCommand.Distance = distance;

            Assert.Throws<InvalidDistanceException>(() => updateConsumptionReportValidator.Validate(stubCommand));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void FuelBurnedLessThanZero_ThrowsInvalidFuelBurnedException(float fuelBurned)
        {
            var stubCommand = GetValidUpdateConsumptionReportCommand();
            stubCommand.FuelBurned = fuelBurned;

            Assert.Throws<InvalidFuelBurnedException>(() => updateConsumptionReportValidator.Validate(stubCommand));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void PricePerUnitLessThanZero_ThrowsInvalidPricePerUnitException(float pricePerUnit)
        {
            var stubCommand = GetValidUpdateConsumptionReportCommand();
            stubCommand.PricePerUnit = pricePerUnit;

            Assert.Throws<InvalidPricePerUnitException>(() => updateConsumptionReportValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyExceptions()
        {
            var stubCommand = GetValidUpdateConsumptionReportCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => updateConsumptionReportValidator.Validate(stubCommand));
        }
    }
}
