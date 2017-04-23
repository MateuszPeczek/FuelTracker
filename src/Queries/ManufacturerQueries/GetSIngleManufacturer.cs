using Common.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Queries.ManufacturerQueries
{
    public class GetSingleManufacturer : IQuery
    {
        public Guid Id { get; set; }
    }

    public class GetSingleManufacturerHandler : IQueryHandler<GetSingleManufacturer, ManufacturerDetails>
    {
        public ManufacturerDetails Handle(GetSingleManufacturer query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"SELECT Id, Name FROM Manufacturer WHERE Id = @id";

                var result = db.Query<ManufacturerDetails>(sqlQuery, new { Id = query.Id }).SingleOrDefault();

                return result;
            }
        }
    }
}
