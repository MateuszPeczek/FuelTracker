using Common.Interfaces;
using Common.Ordering.Engine;
using Common.Ordering.Shared;
using Common.Paging;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Queries.EngineQueries
{
    public class GetEnginesList : IQuery
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public EngineOrderColumn OrderByColumn { get; set; }
        public OrderDirection OrderDirection { get; set; }

        public GetEnginesList(int? pageSize,
                              int? pageNo,
                              EngineOrderColumn orderByolumn,
                              OrderDirection orderDirection)
        {
            PageSize = pageSize ?? 10;
            PageNo = pageNo ?? 1;
            OrderByColumn = orderByolumn;
            OrderDirection = orderDirection;
        }
    }

    public class GetEnginesListHandler : IQueryHandler<GetEnginesList, PaginatedList<EngineDetails>>
    {
        public PaginatedList<EngineDetails> Handle(GetEnginesList query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = $@"SELECT Id, Cylinders, Displacement, FuelType, Name, Power, Torque 
                                 FROM Engine
                                 ORDER BY {query.OrderByColumn.ToString()} {query.OrderDirection.ToString()}
                                 LIMIT {query.PageSize}
                                 OFFSET {query.PageSize * (query.PageNo - 1)}";

                var queryResult = db.Query<EngineDetails>(sqlQuery).ToList();
                

                return new PaginatedList<EngineDetails>() { Data = queryResult, PageNo = query.PageNo, PageSize = query.PageSize };
            }
        }
    }
}
