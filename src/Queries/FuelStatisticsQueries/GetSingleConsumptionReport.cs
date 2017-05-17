using Common.Interfaces;
using CustomExceptions.FuelStatistics;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"SELECT Id, VehicleId DateCreated, Distance, FuelBurned, FuelEfficiency, PricePerUnit, Units FROM ConsumptionReport WHERE VehicleId = @vehicleId AND Id = @fuelReportId";

                var result = db.Query<ConsumptionReportDetails>(sqlQuery, new { VehicleId = query.VehicleId, FuelReportId = query.FuelReportId }).SingleOrDefault();

                if (result == null)
                    throw new ConsumptionReportNotFoundException(query.VehicleId, query.FuelReportId);
                else
                    return result;
            }
        }
    }
}
