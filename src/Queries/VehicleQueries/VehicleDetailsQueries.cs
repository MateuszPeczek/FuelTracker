using Common.Interfaces;
using Dapper;
using Domain.VehicleDomain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Queries.VehicleDetailsQueries
{
    public class GetVehicleDetailsListQuery : IQuery
    {
        
    }

    public class GetVehicleDetailsListQueryHandler : IQueryHandler<GetVehicleDetailsListQuery, ICollection<VehicleDetailsModel>>
    {
        public ICollection<VehicleDetailsModel> Handle(GetVehicleDetailsListQuery query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"select v.guid, mf.name as manufacturer, md.name as model, v.productionyear, e.name as enginename, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join Manufacturer mf on mf.Id = md.ManufacturerID
                                 left join Engine e on e.Id = v.EngineId";
                
                var result = db.Query<VehicleDetailsModel>(sqlQuery).ToList();

                return result;
            }
        }
    }

    public class GetSingleVehicleDetailsQuery : IQuery
    {
        public GetSingleVehicleDetailsQuery(Guid guid)
        {
            this.Guid = guid;
        }

        public Guid Guid { get; set; }
    }

    public class GetSingleVehicleDetailsQueryHandler : IQueryHandler<GetSingleVehicleDetailsQuery, VehicleDetailsModel>
    {
        public VehicleDetailsModel Handle(GetSingleVehicleDetailsQuery query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"select v.guid, mf.name as manufacturer, md.name as model, v.productionyear, e.name as enginename, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join Manufacturer mf on mf.Id = md.ManufacturerID
                                 left join Engine e on e.Id = v.EngineId
                                 where v.Guid = @Guid";


                var result = db.Query<VehicleDetailsModel>(sqlQuery, new { Guid = query.Guid }).SingleOrDefault();

                return result;
            }
        }
    }

    public class VehicleDetailsModel
    {
        public Guid Guid { get; set; }
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
