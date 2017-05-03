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
        public Guid Id { get; set; }

        public GetSingleConsumptionReport(Guid id)
        {
            Id = id;
        }
    }

    public class GetSingleConsumptionReportHandler : IQueryHandler<GetSingleConsumptionReport, ConsumptionReportDetails>
    {
        public ConsumptionReportDetails Handle(GetSingleConsumptionReport query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"SELECT Id, DateCreated, Distance, FuelBurned, FuelEfficency, PricePerUnit, Units FROM ConsumptionReport WHERE Id = @id";

                var result = db.Query<ConsumptionReportDetails>(sqlQuery, new { Id = query.Id }).SingleOrDefault();

                if (result == null)
                    throw new ConsumptionReportNotFoundException(query.Id);
                else
                    return result;
            }
        }
    }
}
