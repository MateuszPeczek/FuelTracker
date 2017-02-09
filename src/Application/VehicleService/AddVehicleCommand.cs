using Common.Interfaces;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.VehicleService
{
    public class AddVehicleCommand : ICommand
    {
        //TODO add proper command fields
        public string Name { get; set; }
        public long EngineID { get; set; }
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
                        EngineID = command.EngineID
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
