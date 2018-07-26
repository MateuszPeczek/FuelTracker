using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using CustomExceptions.User;
using CustomExceptions.Vehicle;
using Domain.Common;
using Domain.FuelStatisticsDomain;
using Persistence;
using System;
using System.Linq;

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
            VehicleId = vehicleId;
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
            if (command.Id == new Guid())
                throw new InvalidCalculateFuelConsumptionIdException();

            if (command.VehicleId == new Guid())
                throw new InvalidVehicleIdException();

            if (command.UserId == new Guid())
                throw new InvalidUserIdException();

            if (command.Distance <= 0)
                throw new InvalidDistanceException();

            if (command.FuelBurned <= 0)
                throw new InvalidFuelBurnedException();

            if (command.PricePerUnit <= 0)
                throw new InvalidPricePerUnitException();
        }
    }

    public class CalculateFuelConsumptionHandler : ICommandHandler<CalculateFuelConsumption>
    {
        private readonly ICommandValidator<CalculateFuelConsumption> commandValidator;
        private readonly IUnitOfWork unitOfWork;

        public CalculateFuelConsumptionHandler(ICommandValidator<CalculateFuelConsumption> commandValidator,
                                               IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.commandValidator = commandValidator;
        }

        public void Handle(CalculateFuelConsumption command)
        {
            commandValidator.Validate(command);

            if (!unitOfWork.Context.Vehicle.Any(v => v.Id == command.VehicleId))
                throw new VehicleNotFoundException(command.VehicleId);

            if (!unitOfWork.Context.User.Any(u => u.Id == command.UserId))
                throw new UserNotFoundException(command.UserId);

            var unitsSettings = unitOfWork.Context.UserSettings.Single(s => s.UserId == command.UserId);
            if (unitsSettings == null)
                throw new UserSettingsNotFoundException(command.UserId);

            var report = new ConsumptionReport()
            {
                Id = command.Id,
                VehicleId = command.VehicleId,
                Distance = command.Distance,
                FuelBurned = command.FuelBurned,
                DateCreated = DateTime.Now,
                PricePerUnit = command.PricePerUnit,
                LastChanged = DateTime.Now
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

            var fuelSummary = unitOfWork.Context.FuelSummary.SingleOrDefault(f => f.VehicleId == command.VehicleId);
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

                switch (fuelSummary.Units)
                {
                    case Units.Metric:
                        fuelSummary.AverageConsumption = (fuelSummary.FuelBurned * 100) / fuelSummary.DistanceDriven;
                        break;
                    case Units.Imperial:
                        fuelSummary.AverageConsumption = fuelSummary.DistanceDriven / fuelSummary.FuelBurned;
                        break;
                }

                unitOfWork.Context.FuelSummary.Add(fuelSummary);
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

                unitOfWork.Context.Entry(fuelSummary).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            unitOfWork.Context.ConsumptionReport.Add(report);
        }
    }
}