using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Paging;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Queries.ManufacturerQueries
{
    public class GetManufacturersList : IQuery
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public OrderDirection OrderDirection { get; set; }

        public GetManufacturersList(int? pageSize,
                                    int? pageNo,
                                    OrderDirection orderDirection)
        {
            PageSize = pageSize ?? 10;
            PageNo = pageNo ?? 1;
            OrderDirection = orderDirection;
        }
    }

    public class GetManufacturersListHandler : IQueryHandler<GetManufacturersList, PaginatedList<ManufacturerDetails>>
    {
        public PaginatedList<ManufacturerDetails> Handle(GetManufacturersList query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = $@"SELECT Id, Name from Manufacturer
                                  ORDER BY Name {query.OrderDirection.ToString()}
                                  LIMIT {query.PageSize}
                                  OFFSET {query.PageSize * (query.PageNo - 1)}";

                var result = db.Query<ManufacturerDetails>(sqlQuery).ToList();

                return new PaginatedList<ManufacturerDetails>() { Data = result, PageNo = query.PageNo, PageSize = query.PageSize };

            }
        }
    }
}
