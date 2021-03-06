﻿using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Ordering.Vehicle;
using Common.Paging;
using Dapper;
using Microsoft.Data.Sqlite;
using Queries.VehicleQueries;
using System.Linq;

namespace Queries.VehicleDetailsQueries
{
    public class GetVehicleDetailsList : IQuery
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public VehicleOrderColumn OrderByColumn { get; set; }
        public OrderDirection OrderDirection { get; set; }

        public GetVehicleDetailsList(int? pageSize, 
                                     int? pageNo, 
                                     VehicleOrderColumn orderByColumn, 
                                     OrderDirection orderDirection)
        {
            PageSize = pageSize ?? 10;
            PageNo = pageNo ?? 1;
            OrderByColumn = orderByColumn;
            OrderDirection = orderDirection;
        }
    }

    public class GetVehicleDetailsListHandler : IQueryHandler<GetVehicleDetailsList, PaginatedList<VehicleDetails>>
    {
        public PaginatedList<VehicleDetails> Handle(GetVehicleDetailsList query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = $@"select  v.id, mf.name as manufacturer, md.name as model, v.productionyear, e.name as enginename, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join manufacturer mf on mf.Id = md.manufacturerId
                                 left join engine e on e.Id = v.EngineId
                                 order by {query.OrderByColumn.ToString()} {query.OrderDirection.ToString()}
                                 LIMIT {query.PageSize}
                                 OFFSET {query.PageSize * (query.PageNo - 1)}";

                var result = db.Query<VehicleDetails>(sqlQuery).ToList();
                
                return new PaginatedList<VehicleDetails>() {Data = result, PageNo = query.PageNo, PageSize = query.PageSize };
            }
        }
    }
}