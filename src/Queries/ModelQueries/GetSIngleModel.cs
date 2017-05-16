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
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
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
