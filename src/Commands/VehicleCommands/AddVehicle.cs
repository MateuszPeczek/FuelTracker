using Common.Interfaces;
using Domain.VehicleDomain;
using Persistence;
using System;

namespace Commands.VehicleCommands
{
    public class AddVehicle : ICommand
    {
        public AddVehicle(Guid modelId)
        {
            Id = Guid.NewGuid();
            ModelId = modelId;
        }

        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
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
                        Id = command.Id,
                        ModelNameId = command.ModelId,
                        
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
