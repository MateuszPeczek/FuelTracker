using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using Domain.Common;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.FuelStatisticsCommands
{
    public class UpdateConsumptionReport : ICommand
    {
        public Guid Id { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float PricePerUnit { get; set; }
    }

    public class UpdateConsumptionReportValidator : ICommandValidator<UpdateConsumptionReport>
    {
        public void Validate(UpdateConsumptionReport command)
        {
            if (command.Id == null || command.Id == new Guid())
                throw new InvalidConsumptionReportIdException();

            if (command.Distance <= 0)
                throw new InvalidDistanceException();

            if (command.FuelBurned <= 0)
                throw new InvalidFuelBurnedException();
        }
    }

    public class UpdateConsumptionReportHandler : ICommandHandler<UpdateConsumptionReport>
    {
        private readonly ICommandValidator<UpdateConsumptionReport> commandValidator;
        private readonly ApplicationContext context;

        private float kilometersToMilesConst = 0.621371F;
        private float milesToKilometersConst = 1.609344F;
        private float litresToGalonsConst = 0.21996916F;
        private float galonsToLitresConst = 4.54609188F;

        public UpdateConsumptionReportHandler(ICommandValidator<UpdateConsumptionReport> commandValidator,
                                               ApplicationContext context)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(UpdateConsumptionReport command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var reportToUpdate = context.ConsumptionReport.SingleOrDefault(r => r.Id == command.Id);
                    if (reportToUpdate == null)
                        throw new ConsumptionReportNotFoundException(command.Id);

                    var fuelSummaryToUpdate = context.FuelSummary.SingleOrDefault(f => f.VehicleId == reportToUpdate.VehicleId);
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
                            fuelSummaryToUpdate.DistanceDriven -= reportToUpdate.Distance * milesToKilometersConst;
                            fuelSummaryToUpdate.FuelBurned -= reportToUpdate.FuelBurned * galonsToLitresConst;
                            fuelSummaryToUpdate.AverageConsumption = (fuelSummaryToUpdate.FuelBurned * 100) / fuelSummaryToUpdate.DistanceDriven;
                        }

                        if (reportToUpdate.Units == Units.Metric && fuelSummaryToUpdate.Units == Units.Imperial)
                        {
                            fuelSummaryToUpdate.DistanceDriven -= reportToUpdate.Distance * kilometersToMilesConst;
                            fuelSummaryToUpdate.FuelBurned -= reportToUpdate.FuelBurned * litresToGalonsConst;
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

                    context.Entry(reportToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.Entry(fuelSummaryToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
