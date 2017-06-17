using Common.Interfaces;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Linq;

namespace Commands.VehicleCommands
{
    public class DeleteVehicle : ICommand
    {
        public DeleteVehicle(Guid guid)
        {
            Id = guid;
        }

        public Guid Id { get; set; }
    }

    public class DeleteVehicleValidator : ICommandValidator<DeleteVehicle>
    {
        public void Validate(DeleteVehicle command)
        {
            if (command.Id == Guid.Empty)
                throw new InvalidVehicleIdException();
        }
    }

    public class DeleteVehicleHandler : ICommandHandler<DeleteVehicle>
    {
        private readonly ApplicationContext context;
        private readonly ICommandValidator<DeleteVehicle> commandValidator;

        public DeleteVehicleHandler(ApplicationContext context, ICommandValidator<DeleteVehicle> commandValidator)
        {
            this.context = context;
            this.commandValidator = commandValidator;
        }

        public void Handle(DeleteVehicle command)
        {
            commandValidator.Validate(command);
            var vehicleToDelete = context.Vehicle.Single(s => s.Id == command.Id);
            var consumptionReportsToDelete = context.ConsumptionReport.Where(c => c.VehicleId == vehicleToDelete.Id);
            var fuelSummaryToDelete = context.FuelSummary.Where(f => f.VehicleId == vehicleToDelete.Id);

            context.Entry(vehicleToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            context.Entry(fuelSummaryToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            foreach (var consumptionReport in consumptionReportsToDelete)
            {
                context.Entry(consumptionReport).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
        }
    }
}