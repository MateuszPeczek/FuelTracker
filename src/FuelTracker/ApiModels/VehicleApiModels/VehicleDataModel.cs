using Domain.VehicleDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelTracker.ApiModels.VehicleApiModels
{
    public class VehicleDataModel
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public EngineDataModel Engine { get; set; }
    }
}
