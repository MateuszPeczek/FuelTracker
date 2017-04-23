using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelTracker.ApiModels.ModelApiModels
{
    public class PutModelName
    {
        public Guid Id { get; set; }
        public Guid ManufactuerId { get; set; }
        public string ModelName { get; set; }
    }
}
