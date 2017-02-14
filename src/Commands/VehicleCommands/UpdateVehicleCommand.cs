using Common.Interfaces;
using Domain.VehicleDomain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commands.VehicleCommands
{
    public class UpdateVehicleCommand : ICommand
    {
        public Guid Guid { get; set; }
        public int? ProductionYear { get; set; }
        public long? EngineId { get; set; }

        public UpdateVehicleCommand(Guid guid, int? productionYear, long? engineId)
        {
            Guid = guid;
            ProductionYear = productionYear;
            EngineId = engineId;
        }
    }

    public class UpdateVehicleCommandHandler : ICommandHandler<UpdateVehicleCommand>
    {
        private readonly ApplicationContext context;

        public UpdateVehicleCommandHandler(ApplicationContext context)
        {
            this.context = context;
        }

        public void Handle(UpdateVehicleCommand command)
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
