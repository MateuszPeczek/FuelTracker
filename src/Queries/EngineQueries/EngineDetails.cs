using Domain.VehicleDomain;

namespace Queries.EngineQueries
{
    public class EngineDetails : BaseDetails
    {
        public string Name { get; set; }
        public int? Power { get; set; }
        public int? Torque { get; set; }
        public int? Cylinders { get; set; }
        public int? Displacement { get; set; }
        public FuelType FuelType { get; set; }
    }
}
