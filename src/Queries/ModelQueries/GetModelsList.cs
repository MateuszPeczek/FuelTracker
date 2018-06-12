using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Paging;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace Queries.ModelQueries
{
    public class GetModelsList : IQuery
    {
        public Guid ManufacturerId { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public OrderDirection OrderDirection { get; set; }

        public GetModelsList(Guid manufacturerId,
                             int? pageSize,
                             int? pageNo,
                             OrderDirection orderDirection)
        {
            ManufacturerId = manufacturerId;
            PageSize = pageSize ?? 10;
            PageNo = pageNo ?? 1;
            OrderDirection = orderDirection;
        }
    }

    public class GetModelsListHandler : IQueryHandler<GetModelsList, PaginatedList<ModelDetails>>
    {
        public PaginatedList<ModelDetails> Handle(GetModelsList query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = $@"SELECT Id, Name from ModelName
                                  WHERE ManufacturerId = '{query.ManufacturerId}'
                                  ORDER BY Name {query.OrderDirection.ToString()}
                                  LIMIT {query.PageSize}
                                  OFFSET {query.PageSize * (query.PageNo - 1)}";

                var result = db.Query<ModelDetails>(sqlQuery).ToList();

                return new PaginatedList<ModelDetails>() { Data = result, PageNo = query.PageNo, PageSize = query.PageSize };

            }
        }
    }
}
