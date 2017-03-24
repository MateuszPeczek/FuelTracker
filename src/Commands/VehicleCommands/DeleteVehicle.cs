using Common.Interfaces;
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

    public class DeleteVehicleHandler : ICommandHandler<DeleteVehicle>
    {
        private readonly ApplicationContext context;

        public DeleteVehicleHandler(ApplicationContext context)
        {
            this.context = context;
        }

        public void Handle(DeleteVehicle command)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var vehicleToDelete = context.Vehicle.Single(s => s.Id == command.Id);
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

