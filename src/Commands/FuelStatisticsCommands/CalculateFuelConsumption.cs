using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using Domain.Common;
using Domain.FuelStatisticsDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands.FuelStatisticsCommands
{
    public class CalculateFuelConsumption : ICommand
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Guid UserId { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public decimal PricePerUnit { get; set; }
    }

    public class CalculateFuelConsumptionValidator : ICommandValidator<CalculateFuelConsumption>
    {
        public void Validate(CalculateFuelConsumption command)
        {
            if (command.VehicleId == null || command.VehicleId == new Guid())
                throw new InvalidVehicleIdException();

            if (command.Distance <= 0)
                throw new InvalidDistanceException();

            if (command.FuelBurned <= 0)
                throw new InvalidFuelBurnedException();
        }
    }

    public class CalculateFuelConsumptionHandler : ICommandHandler<CalculateFuelConsumption>
    {
        private readonly ICommandValidator<CalculateFuelConsumption> commandValidator;
        private readonly ApplicationContext context;

        public CalculateFuelConsumptionHandler(ICommandValidator<CalculateFuelConsumption> commandValidator,
                                               ApplicationContext context)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(CalculateFuelConsumption command)
        {
            commandValidator.Validate(command);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var unitsSettings = context.UserSettings.Single(s => s.UserId == command.UserId);
                    var report = new ConsumptionReport()
                    {
                        Id = command.Id,
                        VehicleId = command.VehicleId,
                        Distance = command.Distance,
                        FuelBurned = command.FuelBurned,
                        UserId = command.UserId,
                        DateCreated = DateTime.Now,
                        PricePerUnit = command.PricePerUnit
                    };
                    
                    switch (unitsSettings.Units)
                    {
                        case Units.Imperial:

                            report.Units = Units.Imperial;

                            report.FuelEfficiency = command.Distance / command.FuelBurned;

                            break;
                        case Units.Metric:

                            report.Units = Units.Metric;

                            report.FuelEfficiency = (command.FuelBurned * 100) / command.Distance;

                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    context.ConsumptionReport.Add(report);

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
