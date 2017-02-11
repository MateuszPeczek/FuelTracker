using Common.Interfaces;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.VehicleApplication
{
    public class AddVehicleCommand : ICommand
    {
        public AddVehicleCommand(long modelID, long manufacturerID)
        {
            Guid = new Guid();
        }

        public Guid Guid { get;}
        public long ModelID { get; set; }
        public long ManufacturerID { get; set; }
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
                        VehicleModelID = command.ModelID,
                        VehicleManufacturerID = command.ManufacturerID
                    };

                    //TODO handle command

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
