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
        public Guid Id { get; set; }

        public GetSingleModel(Guid id)
        {
            Id = id;
        }
    }

    public class GetSingleModelHandler : IQueryHandler<GetSingleModel, ModelDetails>
    {
        public ModelDetails Handle(GetSingleModel query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"SELECT Id, Name FROM ModelName WHERE Id = @id";

                var result = db.Query<ModelDetails>(sqlQuery, new { Id = query.Id }).SingleOrDefault();

                if (result == null)
                    throw new ModelNotFoundException(query.Id);
                else
                    return result;
            }
        }
    }
}
