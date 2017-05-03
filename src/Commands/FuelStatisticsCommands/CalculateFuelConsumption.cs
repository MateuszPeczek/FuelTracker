using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using CustomExceptions.User;
using CustomExceptions.Vehicle;
using Domain.Common;
using Domain.FuelStatisticsDomain;
using Persistence;
using System;
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
        public float PricePerUnit { get; set; }

        public CalculateFuelConsumption(Guid vehicleId, Guid userId, float distance, float fuelBurned, float pricePerUnit)
        {
            Id = Guid.NewGuid();
            VehicleId = VehicleId;
            UserId = userId;
            Distance = distance;
            FuelBurned = fuelBurned;
            PricePerUnit = pricePerUnit;
        }
    }

    public class CalculateFuelConsumptionValidator : ICommandValidator<CalculateFuelConsumption>
    {
        public void Validate(CalculateFuelConsumption command)
        {
            if (command.VehicleId == null || command.VehicleId == new Guid())
                throw new InvalidVehicleIdException();

            if (command.UserId == null || command.UserId == Guid.NewGuid())
                throw new InvalidUserIdException();

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
                    if (!context.Vehicle.Any(v => v.Id == command.VehicleId))
                        throw new VehicleNotFoundException(command.VehicleId);

                    if (!context.User.Any(u => u.Id == command.UserId))
                        throw new UserNotFoundException(command.UserId);

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

                    var fuelSummary = context.FuelSummary.SingleOrDefault(f => f.VehicleId == command.VehicleId);
                    if (fuelSummary == null)
                    {
                        fuelSummary = new FuelSummary() { Id = Guid.NewGuid() };

                        fuelSummary.DistanceDriven = report.Distance;
                        fuelSummary.FuelBurned = report.FuelBurned;
                        fuelSummary.MoneySpent = report.PricePerUnit * report.FuelBurned;
                        fuelSummary.ReportsNumber = 1;
                        fuelSummary.VehicleId = command.VehicleId;
                        fuelSummary.AverageConsumption = report.FuelEfficiency;
                        fuelSummary.Units = report.Units;

                        context.FuelSummary.Add(fuelSummary);
                    }
                    else
                    {
                        fuelSummary.ReportsNumber++;
                        fuelSummary.DistanceDriven += report.Distance;
                        fuelSummary.FuelBurned += report.FuelBurned;
                        fuelSummary.MoneySpent += report.PricePerUnit * report.FuelBurned;

                        switch (unitsSettings.Units)
                        {
                            case Units.Imperial:
                                fuelSummary.Units = Units.Imperial;
                                fuelSummary.AverageConsumption = fuelSummary.DistanceDriven / fuelSummary.FuelBurned;
                                break;
                            case Units.Metric:
                                report.Units = Units.Metric;
                                report.FuelEfficiency = (fuelSummary.FuelBurned * 100) / fuelSummary.DistanceDriven;
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        context.Entry(fuelSummary).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
