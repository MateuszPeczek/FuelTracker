using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelTracker.ApiModels.Vehicle
{
    public class PostVehicle
    {
        public long ManufacturerID { get; set; }
        public long ModelID { get; set; }
    }
}
