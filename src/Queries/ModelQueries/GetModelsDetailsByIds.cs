using Common.Interfaces;
using CustomExceptions.Manufacturer;
using CustomExceptions.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Queries.ModelQueries
{
    public class GetModelsDetailsByIds : IQuery
    {
        public Guid ManufacturerId { get; set; }
        public IEnumerable<Guid> Ids { get; set; }

        public GetModelsDetailsByIds(Guid manufacturerId, IEnumerable<Guid> ids)
        {
            ManufacturerId = manufacturerId;
            Ids = ids;
        }
    }

    public class GetModelsByIdsHandler : IQueryHandler<GetModelsDetailsByIds, IEnumerable<ModelDetails>>
    {
        public IEnumerable<ModelDetails> Handle(GetModelsDetailsByIds query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = $@"SELECT Id, Name FROM ModelName WHERE ManufacturerId = '{query.ManufacturerId}' and Id IN @ids";

                var result = db.Query<ModelDetails>(sqlQuery, new { ManufacturerId = query.ManufacturerId, Ids = query.Ids }).ToList();
                
                return result;
            }
        }
    }
}
