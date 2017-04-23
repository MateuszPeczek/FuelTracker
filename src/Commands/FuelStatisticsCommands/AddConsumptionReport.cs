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
    public class AddConsumptionReport : ICommand
    {
        private Guid _guid;
        public Guid Id
        {
            get
            {
                if (_guid == null || _guid == new Guid())
                    return Guid.NewGuid();
                else
                    return _guid;
            }
        }
        public Guid VehicleId { get; set; }
        public Guid UserId { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float FuelEfficiency { get; set; }
        public decimal PricePerUnit { get; set; }
        public Units Units { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class AddConsumptionReportValidator : ICommandValidator<AddConsumptionReport>
    {
        public void Validate(AddConsumptionReport command)
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

    public class AddConsumptionReportHandler : ICommandHandler<AddConsumptionReport>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<AddConsumptionReport> commandValidator;

        public AddConsumptionReportHandler(ICommandValidator<AddConsumptionReport> commandValidator,
            ApplicationContext context)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(AddConsumptionReport command)
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

                    var reportToAdd = new ConsumptionReport()
                    {
                        DateCreated = command.DateCreated,
                        Distance = command.Distance,
                        FuelBurned = command.FuelBurned,
                        FuelEfficiency = command.FuelEfficiency,
                        Id = command.Id,
                        PricePerUnit = command.PricePerUnit,
                        Units = command.Units,
                        UserId = command.UserId,
                        VehicleId = command.VehicleId
                    };

                    context.ConsumptionReport.Add(reportToAdd);

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
