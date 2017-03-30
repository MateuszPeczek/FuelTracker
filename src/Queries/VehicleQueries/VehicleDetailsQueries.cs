using Common.Interfaces;
using Common.Ordering.Shared;
using Common.Ordering.Vehicle;
using Common.Paging;
using Dapper;
using Domain.VehicleDomain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                

                var sqlQuery = $@"select  v.id, mf.name as manufacturer, md.name as model, v.productionyear, e.name as enginename, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join manufacturer mf on mf.Id = md.manufacturerId
                                 left join engine e on e.Id = v.EngineId
                                 order by {query.OrderByColumn.ToString()} {query.OrderDirection.ToString()}
                                 offset {query.PageSize * (query.PageNo - 1)} rows 
                                 fetch next {query.PageSize} rows only";
                
                var result = db.Query<VehicleDetails>(sqlQuery).ToList();
                
                return new PaginatedList<VehicleDetails>() {Data = result, PageNo = query.PageNo, PageSize = query.PageSize };
            }
        }
    }

    public class GetSingleVehicleDetails : IQuery
    {
        public GetSingleVehicleDetails(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }

    public class GetSingleVehicleDetailsHandler : IQueryHandler<GetSingleVehicleDetails, VehicleDetails>
    {
        public VehicleDetails Handle(GetSingleVehicleDetails query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"select v.id, mf.name as manufacturer, md.name as model, v.productionyear, e.name as enginename, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join Manufacturer mf on mf.Id = md.ManufacturerID
                                 left join Engine e on e.Id = v.EngineId
                                 where v.id = @id";

                var result = db.Query<VehicleDetails>(sqlQuery, new { Id = query.Id }).SingleOrDefault();

                return result;
            }
        }
    }

    public class VehicleDetails
    {
        public Guid Id { get; set; }
        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public int ProductionYear { get; set; }

        public string EngineName { get; set; }

        public int Power { get; set; }

        public int Torque { get; set; }

        public int Cylinders { get; set; }

        public float Displacement { get; set; }

        public FuelType FuelType { get; set; }
    }
}
