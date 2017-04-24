using Common.Interfaces;
using CustomExceptions.Engine;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Queries.EngineQueries
{
    public class GetSingleEngine : IQuery
    {
        public Guid Id { get; set; }

        public GetSingleEngine(Guid id)
        {
            Id = id;
        }
    }

    public class GetSingleEngineHandler : IQueryHandler<GetSingleEngine, EngineDetails>
    {
        public EngineDetails Handle(GetSingleEngine query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = $@"SELECT Id, Cylinders, Displacement, FuelType, Name, Power, Torque 
                                 FROM Engine
                                 WHERE ID = @id";

                var result = db.Query<EngineDetails>(sqlQuery, new { Id = query.Id }).SingleOrDefault();

                if (result == null)
                    throw new EngineNotFoundException(query.Id);
                else
                    return result;
            }
        }
    }
}
