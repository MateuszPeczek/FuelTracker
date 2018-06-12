using Common.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

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
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = @"SELECT Id, Name FROM Manufacturer WHERE Id IN @ids";

                var result = db.Query<ManufacturerDetails>(sqlQuery, new { ids = query.Ids }).ToList();

                return result;
            }
        }
    }
}
