using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Paging;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = $@"SELECT Id, Name from Manufacturer
                                  ORDER BY Name {query.OrderDirection.ToString()}
                                  OFFSET {query.PageSize * (query.PageNo - 1)} ROWS
                                  FETCH NEXT {query.PageSize} ROWS ONLY";

                var result = db.Query<ManufacturerDetails>(sqlQuery).ToList();

                return new PaginatedList<ManufacturerDetails>() { Data = result, PageNo = query.PageNo, PageSize = query.PageSize };

            }
        }
    }
}
