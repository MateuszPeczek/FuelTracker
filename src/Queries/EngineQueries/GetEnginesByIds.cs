using Common.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Queries.EngineQueries
{
    public class GetEnginesByIds : IQuery
    {
        public IEnumerable<Guid> EngineIds { get; set; }

        public GetEnginesByIds(IEnumerable<Guid> engineIds)
        {
            EngineIds = engineIds;
        }
    }

    public class GetEnginesByIdsHandler : IQueryHandler<GetEnginesByIds, IEnumerable<EngineDetails>>
    {

        public IEnumerable<EngineDetails> Handle(GetEnginesByIds query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = $@"SELECT Id, Cylinders, Displacement, FuelType, Name, Power, Torque 
                                 FROM Engine
                                 WHERE ID IN @ids";

                var result = db.Query<EngineDetails>(sqlQuery, new { ids = query.EngineIds }).AsList();

                return result;
            }
        }
    }
}
