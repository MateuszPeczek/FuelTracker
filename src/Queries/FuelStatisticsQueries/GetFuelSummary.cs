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
    public class GetFuelSummary : IQuery
    {
        public Guid VehicleId { get; set; }
    }

    public class GetFuelSummaryHandler : IQueryHandler<GetFuelSummary, FuelSummaryDetails>
    {
        public FuelSummaryDetails Handle(GetFuelSummary query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"SELECT Id, AverageConsumption, DistanceDriven, FuelBurned, MoneySpent, ReportsNumber FROM ConsumptionReport WHERE VehicleId = @vehicleId";

                var result = db.Query<FuelSummaryDetails>(sqlQuery, new { VehicleId = query.VehicleId }).SingleOrDefault();

                if (result == null)
                    throw new FuelSummaryNotFoundException(query.VehicleId);
                else
                    return result;
            }
        }
    }
}
