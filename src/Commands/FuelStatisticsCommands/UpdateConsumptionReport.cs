using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using CustomExceptions.Vehicle;
using Domain.Common;
using Persistence;
using System;
using System.Linq;

namespace Commands.FuelStatisticsCommands
{
    public class UpdateConsumptionReport : ICommand
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float PricePerUnit { get; set; }

        public UpdateConsumptionReport(Guid consumptionReportId, Guid vehicleId, float distance, float fuelBurned, float pricePerUnit)
        {
            Id = consumptionReportId;
            VehicleId = vehicleId;
            Distance = distance;
            FuelBurned = fuelBurned;
            PricePerUnit = pricePerUnit;
        }
    }

    public class UpdateConsumptionReportValidator : ICommandValidator<UpdateConsumptionReport>
    {
        public void Validate(UpdateConsumptionReport command)
        {
            if (command.Id == new Guid())
                throw new InvalidConsumptionReportIdException();

            if (command.VehicleId == new Guid())
                throw new InvalidVehicleIdException();

            if (command.Distance <= 0)
                throw new InvalidDistanceException();

            if (command.FuelBurned <= 0)
                throw new InvalidFuelBurnedException();

            if (command.PricePerUnit <= 0)
                throw new InvalidPricePerUnitException();
        }
    }

    public class UpdateConsumptionReportHandler : ICommandHandler<UpdateConsumptionReport>
    {
        private readonly ICommandValidator<UpdateConsumptionReport> commandValidator;
        private readonly IUnitOfWork unitOfWork;

        private const float kilometersToMilesConst = 0.621371F;
        private const float milesToKilometersConst = 1.609344F;
        private const float litresToGalonsConst = 0.21996916F;
        private const float galonsToLitresConst = 4.54609188F;

        public UpdateConsumptionReportHandler(ICommandValidator<UpdateConsumptionReport> commandValidator,
                                              IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateConsumptionReport command)
        {
            commandValidator.Validate(command);

            var reportToUpdate = unitOfWork.Context.ConsumptionReport.SingleOrDefault(r => r.Id == command.Id);
            if (reportToUpdate == null)
                throw new ConsumptionReportNotFoundException(command.VehicleId, command.Id);

            var fuelSummaryToUpdate = unitOfWork.Context.FuelSummary.SingleOrDefault(f => f.VehicleId == reportToUpdate.VehicleId);
            if (fuelSummaryToUpdate == null)
                throw new FuelSummaryNotFoundException(reportToUpdate.VehicleId);


            if (reportToUpdate.Units == fuelSummaryToUpdate.Units)
            {
                fuelSummaryToUpdate.DistanceDriven -= reportToUpdate.Distance - command.Distance;
                fuelSummaryToUpdate.FuelBurned -= reportToUpdate.FuelBurned - command.FuelBurned;
                fuelSummaryToUpdate.MoneySpent -= (reportToUpdate.PricePerUnit * reportToUpdate.FuelBurned) - (command.PricePerUnit * command.FuelBurned);

                switch (fuelSummaryToUpdate.Units)
                {
                    case Units.Metric:
                        fuelSummaryToUpdate.AverageConsumption = (fuelSummaryToUpdate.FuelBurned * 100) / fuelSummaryToUpdate.DistanceDriven;
                        break;
                    case Units.Imperial:
                        fuelSummaryToUpdate.AverageConsumption = fuelSummaryToUpdate.DistanceDriven / fuelSummaryToUpdate.FuelBurned;
                        break;
                }
            }
            else
            {
                if (reportToUpdate.Units == Units.Imperial && fuelSummaryToUpdate.Units == Units.Metric)
                {
                    fuelSummaryToUpdate.DistanceDriven -= (reportToUpdate.Distance - command.Distance) * milesToKilometersConst;
                    fuelSummaryToUpdate.FuelBurned -= (reportToUpdate.FuelBurned - command.FuelBurned) * galonsToLitresConst;
                    fuelSummaryToUpdate.AverageConsumption = (fuelSummaryToUpdate.FuelBurned * 100) / fuelSummaryToUpdate.DistanceDriven;
                }

                if (reportToUpdate.Units == Units.Metric && fuelSummaryToUpdate.Units == Units.Imperial)
                {
                    fuelSummaryToUpdate.DistanceDriven -= (reportToUpdate.Distance - command.Distance) * kilometersToMilesConst;
                    fuelSummaryToUpdate.FuelBurned -= (reportToUpdate.FuelBurned - command.FuelBurned) * litresToGalonsConst;
                    fuelSummaryToUpdate.AverageConsumption = fuelSummaryToUpdate.DistanceDriven / fuelSummaryToUpdate.FuelBurned;
                }
            }

            reportToUpdate.Distance = command.Distance;
            reportToUpdate.FuelBurned = command.FuelBurned;
            reportToUpdate.PricePerUnit = command.PricePerUnit;
            reportToUpdate.LastChanged = DateTime.Now;

            switch (reportToUpdate.Units)
            {
                case Units.Metric:
                    reportToUpdate.FuelEfficiency = (reportToUpdate.FuelBurned * 100) / reportToUpdate.Distance;
                    break;
                case Units.Imperial:
                    reportToUpdate.FuelEfficiency = reportToUpdate.Distance / reportToUpdate.FuelBurned;
                    break;
            }

            unitOfWork.Context.Entry(reportToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            unitOfWork.Context.Entry(fuelSummaryToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}