using Common.Interfaces;

namespace Domain.VehicleDomain
{
    public class Vehicle : IEntity
    {
        public long Id { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Model Model { get; set; }
        public int ProductionYear { get; set; }
        public Engine Engine { get; set; }
        public VehicleType Type { get; set; }
        public long UserId { get; set; }

    }
}
