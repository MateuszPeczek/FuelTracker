using Common.Interfaces;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commands.VehicleCommands
{
    public class UpdateVehicle : ICommand
    {
        public Guid Guid { get; set; }
        public int? ProductionYear { get; set; }
        public long? EngineId { get; set; }

        public UpdateVehicle(Guid guid, int? productionYear, long? engineId)
        {
            Guid = guid;
            ProductionYear = productionYear;
            EngineId = engineId;
        }
    }

    public class UpdateVehicleHandler : ICommandHandler<UpdateVehicle>
    {
        private readonly ApplicationContext context;

        public UpdateVehicleHandler(ApplicationContext context)
        {
            this.context = context;
        }

        public void Handle(UpdateVehicle command)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var vehicleToUpdate = context.Vehicle.Single(v => v.Guid == command.Guid);
                    var selectedEngine = context.Engine.FirstOrDefault(e => e.Id == command.EngineId);

                    vehicleToUpdate.ProductionYear = command.ProductionYear;

                    if (selectedEngine != null)
                        vehicleToUpdate.Engine = selectedEngine;

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
