using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using Domain.Common;
using Persistence;
using System;
using System.Linq;

namespace Commands.FuelStatisticsCommands
{
    public class DeleteConsumptionReport : ICommand
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }

        public DeleteConsumptionReport(Guid vehicleId, Guid fuelReportId)
        {
            Id = fuelReportId;
            VehicleId = vehicleId;
        }
    }

    public class DeleteConsumptionReportValidator : ICommandValidator<DeleteConsumptionReport>
    {
        public void Validate(DeleteConsumptionReport command)
        {
            if (command.Id == new Guid())
                throw new InvalidConsumptionReportIdException();
        }
    }

    public class DeleteConsumptionReportHandler : ICommandHandler<DeleteConsumptionReport>
    {
        private readonly ICommandValidator<DeleteConsumptionReport> commandValidator;
        private readonly IUnitOfWork unitOfWork;

        private const float kilometersToMilesConst = 0.621371F;
        private const float milesToKilometersConst = 1.609344F;
        private const float litresToGalonsConst = 0.21996916F;
        private const float galonsToLitresConst = 4.54609188F;

        public DeleteConsumptionReportHandler(ICommandValidator<DeleteConsumptionReport> commandValidator,
                                              IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteConsumptionReport command)
        {
            commandValidator.Validate(command);

            var reportToDelete = unitOfWork.Context.ConsumptionReport.Where(r => r.VehicleId == command.VehicleId).SingleOrDefault(r => r.Id == command.Id);
            if (reportToDelete == null)
                throw new ConsumptionReportNotFoundException(command.VehicleId, command.Id);

            var fuelSummaryToUpdate = unitOfWork.Context.FuelSummary.SingleOrDefault(f => f.VehicleId == reportToDelete.VehicleId);
            if (fuelSummaryToUpdate == null)
                throw new FuelSummaryNotFoundException(reportToDelete.VehicleId);

            if (reportToDelete.Units == fuelSummaryToUpdate.Units)
            {
                fuelSummaryToUpdate.DistanceDriven -= reportToDelete.Distance;
                fuelSummaryToUpdate.FuelBurned -= reportToDelete.FuelBurned;
                fuelSummaryToUpdate.MoneySpent -= (reportToDelete.PricePerUnit * reportToDelete.FuelBurned);

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
                if (reportToDelete.Units == Units.Imperial && fuelSummaryToUpdate.Units == Units.Metric)
                {
                    fuelSummaryToUpdate.DistanceDriven -= reportToDelete.Distance * milesToKilometersConst;
                    fuelSummaryToUpdate.FuelBurned -= reportToDelete.FuelBurned * galonsToLitresConst;
                    fuelSummaryToUpdate.AverageConsumption = (fuelSummaryToUpdate.FuelBurned * 100) / fuelSummaryToUpdate.DistanceDriven;
                }

                if (reportToDelete.Units == Units.Metric && fuelSummaryToUpdate.Units == Units.Imperial)
                {
                    fuelSummaryToUpdate.DistanceDriven -= reportToDelete.Distance * kilometersToMilesConst;
                    fuelSummaryToUpdate.FuelBurned -= reportToDelete.FuelBurned * litresToGalonsConst;
                    fuelSummaryToUpdate.AverageConsumption = fuelSummaryToUpdate.DistanceDriven / fuelSummaryToUpdate.FuelBurned;
                }
            }

            unitOfWork.Context.Entry(reportToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            unitOfWork.Context.Entry(fuelSummaryToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}