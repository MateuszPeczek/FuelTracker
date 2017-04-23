using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Paging;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Queries.ModelQueries
{
    public class GetModelsList : IQuery
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public OrderDirection OrderDirection { get; set; }

        public GetModelsList(int? pageSize,
                             int? pageNo,
                             OrderDirection orderDirection)
        {
            PageSize = pageSize ?? 10;
            PageNo = pageNo ?? 1;
            OrderDirection = orderDirection;
        }
    }

    public class GetModelsListHandler : IQueryHandler<GetModelsList, PaginatedList<ModelDetails>>
    {
        public PaginatedList<ModelDetails> Handle(GetModelsList query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = $@"SELECT Id, Name from ModelName
                                  ORDER BY Name {query.OrderDirection.ToString()}
                                  OFFSET {query.PageSize * (query.PageNo - 1)} ROWS
                                  FETCH NEXT {query.PageSize} ROWS ONLY";

                var result = db.Query<ModelDetails>(sqlQuery).ToList();

                return new PaginatedList<ModelDetails>() { Data = result, PageNo = query.PageNo, PageSize = query.PageSize };

            }
        }
    }
}
