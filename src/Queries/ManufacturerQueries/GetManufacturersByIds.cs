using Common.Interfaces;
using CustomExceptions.Manufacturer;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Queries.ManufacturerQueries
{
    public class GetManufacturersByIds : IQuery
    {
        public IEnumerable<Guid> Ids { get; set; }

        public GetManufacturersByIds(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }

    public class GetManufacturersByIdsHandler : IQueryHandler<GetManufacturersByIds, IEnumerable<ManufacturerDetails>>
    {
        public IEnumerable<ManufacturerDetails> Handle(GetManufacturersByIds query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"SELECT Id, Name FROM Manufacturer WHERE Id IN @ids";

                var result = db.Query<ManufacturerDetails>(sqlQuery, new { ids = query.Ids }).ToList();

                return result;
            }
        }
    }
}
