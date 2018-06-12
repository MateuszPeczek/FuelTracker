using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace Queries.FuelStatisticsQueries
{
    public class GetSingleConsumptionReport : IQuery
    {
        public Guid VehicleId { get; set; }
        public Guid FuelReportId { get; set; }

        public GetSingleConsumptionReport(Guid vehicleId, Guid fuelReportId)
        {
            VehicleId = vehicleId;
            FuelReportId = fuelReportId;
        }
    }

    public class GetSingleConsumptionReportHandler : IQueryHandler<GetSingleConsumptionReport, ConsumptionReportDetails>
    {
        public ConsumptionReportDetails Handle(GetSingleConsumptionReport query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = @"SELECT Id, VehicleId, DateCreated, Distance, FuelBurned, FuelEfficiency, PricePerUnit, Units FROM ConsumptionReport WHERE VehicleId = @VehicleId AND Id = @FuelReportId";

                var result = db.Query<ConsumptionReportDetails>(sqlQuery, new { VehicleId = query.VehicleId, FuelReportId = query.FuelReportId }).SingleOrDefault();

                if (result == null)
                    throw new ConsumptionReportNotFoundException(query.VehicleId, query.FuelReportId);
                else
                    return result;
            }
        }
    }
}
