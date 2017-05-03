﻿using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Paging;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Queries.FuelStatisticsQueries
{
    public class GetConsumptionReportsList : IQuery
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public OrderDirection OrderDirection { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public GetConsumptionReportsList(int? pageSize,
                                    int? pageNo,
                                    OrderDirection orderDirection,
                                    DateTime? startDate = null,
                                    DateTime? endDate = null)
        {
            PageSize = pageSize ?? 10;
            PageNo = pageNo ?? 1;
            OrderDirection = orderDirection;
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    public class GetConsumptionReportsListHandler : IQueryHandler<GetConsumptionReportsList, PaginatedList<ConsumptionReportDetails>>
    {
        public PaginatedList<ConsumptionReportDetails> Handle(GetConsumptionReportsList query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var startDateFilterValue = query.StartDate.HasValue ? query.StartDate.Value : DateTime.MinValue;
                var endDateFilterValue = query.EndDate.HasValue ? query.EndDate.Value : DateTime.MaxValue;

                var sqlQuery = $@"SELECT Id, DateCreated, Distance, FuelBurned, FuelEfficency, PricePerUnit, Units FROM ConsumptionReport
                                  WHERE DateCreated > {startDateFilterValue.ToString()} AND DateCreated < {endDateFilterValue.ToString()}
                                  ORDER BY Name {query.OrderDirection.ToString()}
                                  OFFSET {query.PageSize * (query.PageNo - 1)} ROWS
                                  FETCH NEXT {query.PageSize} ROWS ONLY";

                var result = db.Query<ConsumptionReportDetails>(sqlQuery).ToList();

                return new PaginatedList<ConsumptionReportDetails>() { Data = result, PageNo = query.PageNo, PageSize = query.PageSize };

            }
        }
    }
}