using Common.Interfaces;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commands.VehicleCommands
{
    public class DeleteVehicleCommand : ICommand
    {
        public DeleteVehicleCommand(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; set; }
    }

    public class DeleteVehicleCommandHandler : ICommandHandler<DeleteVehicleCommand>
    {
        private readonly ApplicationContext context;

        public DeleteVehicleCommandHandler(ApplicationContext context)
        {
            this.context = context;
        }

        public void Handle(DeleteVehicleCommand command)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var vehicleToDelete = context.Vehicle.Single(s => s.Guid == command.Guid);
                    context.Entry(vehicleToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

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

