﻿using Common.Interfaces;
using Common.Ordering.FuelStatistics;
using Common.Ordering.Shared;
using Common.Paging;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace Queries.FuelStatisticsQueries
{
    public class GetConsumptionReportsList : IQuery
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public OrderDirection OrderDirection { get; set; }
        public ConsumptionReportOrderColumn OrderColumn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public GetConsumptionReportsList(int? pageSize,
                                    int? pageNo,
                                    OrderDirection orderDirection,
                                    ConsumptionReportOrderColumn OrderColumn,
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
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var startDateFilterValue = query.StartDate ?? DateTime.MinValue;
                var endDateFilterValue = query.EndDate ?? DateTime.MaxValue;

                var sqlQuery = $@"SELECT Id, VehicleId, DateCreated, Distance, FuelBurned, FuelEfficiency, PricePerUnit, Units FROM ConsumptionReport
                                  WHERE DateCreated > '{startDateFilterValue.ToString("yyyy-MM-dd HH:mm:ss.fff")}' AND DateCreated < '{endDateFilterValue.ToString("yyyy-MM-dd HH:mm:ss.fff")}'
                                  ORDER BY {query.OrderColumn.ToString()} {query.OrderDirection.ToString()}
                                  LIMIT {query.PageSize}
                                  OFFSET {query.PageSize * (query.PageNo - 1)}";

                var result = db.Query<ConsumptionReportDetails>(sqlQuery).ToList();

                return new PaginatedList<ConsumptionReportDetails>() { Data = result, PageNo = query.PageNo, PageSize = query.PageSize };

            }
        }
    }
}
