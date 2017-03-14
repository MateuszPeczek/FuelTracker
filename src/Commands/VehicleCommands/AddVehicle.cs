using Common.Interfaces;
using Domain.VehicleDomain;
using Persistence;
using System;

namespace Commands.VehicleCommands
{
    public class AddVehicle : ICommand
    {
        public AddVehicle(long modelId)
        {
            this.Guid = Guid.NewGuid();
            this.ModelId = modelId;
        }

        public Guid Guid { get; set; }
        public long ModelId { get; set; }
    }

    public class AddVehicleHandler : ICommandHandler<AddVehicle>
    {
        private readonly ApplicationContext context;

        public AddVehicleHandler(ApplicationContext context)
        {
            this.context = context;
        }

        public void Handle(AddVehicle command)
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
