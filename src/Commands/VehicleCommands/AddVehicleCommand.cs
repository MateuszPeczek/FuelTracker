using Common.Interfaces;
using Domain.VehicleDomain;
using Persistence;
using System;

namespace Commands.VehicleCommands
{
    public class AddVehicleCommand : ICommand
    {
        public AddVehicleCommand(long modelId)
        {
            this.Guid = Guid.NewGuid();
            this.ModelId = modelId;
        }

        public Guid Guid { get; set; }
        public long ModelId { get; set; }
    }

    public class AddVehicleCommandHandler : ICommandHandler<AddVehicleCommand>
    {
        private readonly ApplicationContext context;

        public AddVehicleCommandHandler(ApplicationContext context)
        {
            this.context = context;
        }

        public void Handle(AddVehicleCommand command)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var newVehicle = new Vehicle()
                    {
                        Guid = command.Guid,
                        ModelNameId = command.ModelId,
                        //UserId = 1
                    };

                    context.Vehicle.Add(newVehicle);

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
