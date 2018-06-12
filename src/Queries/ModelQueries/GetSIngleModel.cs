using Common.Interfaces;
using CustomExceptions.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace Queries.ModelQueries
{
    public class GetSingleModel : IQuery
    {
        public Guid ManufacturerId { get; set; }
        public Guid ModelId { get; set; }

        public GetSingleModel(Guid manufacturerId, Guid modelId)
        {
            ManufacturerId = manufacturerId;
            ModelId = modelId;
        }
    }

    public class GetSingleModelHandler : IQueryHandler<GetSingleModel, ModelDetails>
    {
        public ModelDetails Handle(GetSingleModel query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = $@"SELECT Id, Name FROM ModelName WHERE ManufacturerId = '{query.ManufacturerId}' and Id = '{query.ModelId}'";

                var result = db.Query<ModelDetails>(sqlQuery, new { ManufacturerId = query.ManufacturerId, ModelId = query.ModelId }).SingleOrDefault();

                if (result == null)
                    throw new ModelNotFoundException(query.ManufacturerId, query.ModelId);
                else
                    return result;
            }
        }
    }
}
