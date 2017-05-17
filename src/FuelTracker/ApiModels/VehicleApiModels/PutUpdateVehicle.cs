using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelTracker.ApiModels.VehicleApiModels
{
    public class PutUpdateVehicle
    {
        public int? ProductionYear { get; set; }
        public Guid? EngineId { get; set; }
    }
}
