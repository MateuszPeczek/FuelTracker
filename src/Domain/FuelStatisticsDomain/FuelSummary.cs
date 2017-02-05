using Common.Interfaces;

namespace Domain.FuelStatisticsDomain
{
    public class FuelSummary : IEntity
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public float AverageConsumption { get; set; }
        public long ReportsNumber { get; set; }
        public decimal DistanceDriven { get; set; }
        public decimal FuelBurned { get; set; }
        public decimal MoneySpent { get; set; }
    }
}
