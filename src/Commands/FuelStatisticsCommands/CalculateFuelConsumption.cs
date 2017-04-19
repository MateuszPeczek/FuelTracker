using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commands.FuelStatisticsCommands
{
    public class CalculateFuelConsumption : ICommand
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
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
        private readonly ICommandHandler<CalculateFuelConsumption> commandValidator;
        private readonly ApplicationContext context;

        public CalculateFuelConsumptionHandler(ICommandHandler<CalculateFuelConsumption> commandValidator,
                                               ApplicationContext context)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(CalculateFuelConsumption command)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {


                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
