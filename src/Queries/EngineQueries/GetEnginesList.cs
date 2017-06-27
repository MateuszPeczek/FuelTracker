using Common.Interfaces;
using Common.Ordering.Engine;
using Common.Ordering.Shared;
using Common.Paging;
using Dapper;
using System.Data.SqlClient;
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
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = $@"SELECT Id, Cylinders, Displacement, FuelType, Name, Power, Torque 
                                 FROM Engine
                                 ORDER BY {query.OrderByColumn.ToString()} {query.OrderDirection.ToString()}
                                 OFFSET {query.PageSize * (query.PageNo - 1)} ROWS 
                                 FETCH NEXT {query.PageSize} ROWS ONLY";

                var result = db.Query<EngineDetails>(sqlQuery).ToList();

                return new PaginatedList<EngineDetails>() { Data = result, PageNo = query.PageNo, PageSize = query.PageSize };
            }
        }
    }
}
