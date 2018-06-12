using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace Queries.FuelStatisticsQueries
{
    public class GetFuelSummary : IQuery
    {
        public Guid VehicleId { get; set; }

        public GetFuelSummary(Guid vehicleId)
        {
            VehicleId = vehicleId;
        }
    }

    public class GetFuelSummaryHandler : IQueryHandler<GetFuelSummary, FuelSummaryDetails>
    {
        public FuelSummaryDetails Handle(GetFuelSummary query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = @"SELECT Id, VehicleId, AverageConsumption, DistanceDriven, FuelBurned, MoneySpent, ReportsNumber FROM FuelSummary WHERE VehicleId = @vehicleId";

                var result = db.Query<FuelSummaryDetails>(sqlQuery, new { VehicleId = query.VehicleId }).SingleOrDefault();

                if (result == null)
                    throw new FuelSummaryNotFoundException(query.VehicleId);
                else
                    return result;
            }
        }
    }
}
