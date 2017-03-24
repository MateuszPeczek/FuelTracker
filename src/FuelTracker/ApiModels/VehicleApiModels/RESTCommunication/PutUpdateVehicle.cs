using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelTracker.ApiModels.VehicleApiModels.RESTCommunication
{
    public class PutUpdateVehicle
    {
        public Guid Guid { get; set; }
        public int? ProductionYear { get; set; }
        public Guid? EngineId { get; set; }
    }
}
