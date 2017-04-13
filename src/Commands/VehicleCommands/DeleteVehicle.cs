using Common.Interfaces;
using CustomExceptions.Vehicle;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var vehicleToDelete = context.Vehicle.Single(s => s.Id == command.Id);
                    context.Entry(vehicleToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

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

