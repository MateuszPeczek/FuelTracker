using Commands.FuelStatisticsCommands;
using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using CustomExceptions.Vehicle;
using System;
using UnitTests.Extensions;
using Xunit;

namespace UnitTests.Validators.FuelStatistics
{
    public class DeleteConsumptionReportValidatorTests
    {
        private readonly ICommandValidator<DeleteConsumptionReport> deleteConsumtionReportValidator;

        public DeleteConsumptionReportValidatorTests()
        {
            deleteConsumtionReportValidator = new DeleteConsumptionReportValidator();
        }

        #region Setup
        
        public DeleteConsumptionReport GetValidCeleteConsumptionReportCommand()
        {
            return new DeleteConsumptionReport(Guid.NewGuid(), Guid.NewGuid());
        }

        #endregion

        [Fact]
        public void EmptyConsumptionReportId_ThrowsInvalidConsumptionReportId()
        {
            var stubCommand = GetValidCeleteConsumptionReportCommand();
            stubCommand.Id = new Guid();

            Assert.Throws<InvalidConsumptionReportIdException>(() => deleteConsumtionReportValidator.Validate(stubCommand));
        }

        [Fact]
        public void EmptyVehicleId_ThrowsInvalidVehicleIdException()
        {
            var stubCommand = GetValidCeleteConsumptionReportCommand();
            stubCommand.VehicleId = new Guid();

            Assert.Throws<InvalidVehicleIdException>(() => deleteConsumtionReportValidator.Validate(stubCommand));
        }

        [Fact]
        public void ValidCommand_NotThrowsAnyException()
        {
            var stubCommand = GetValidCeleteConsumptionReportCommand();

            ExtendedAssert.NotThrowsAnyExceptions(() => deleteConsumtionReportValidator.Validate(stubCommand));
        }
    }
}
