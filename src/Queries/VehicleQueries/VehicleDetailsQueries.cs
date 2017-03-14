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
    public class GetVehicleDetailsList : IQuery
    {
        
    }

    public class GetVehicleDetailsListHandler : IQueryHandler<GetVehicleDetailsList, ICollection<VehicleDetails>>
    {
        public ICollection<VehicleDetails> Handle(GetVehicleDetailsList query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"select v.guid, mf.name as manufacturer, md.name as model, v.productionyear, e.name as enginename, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join Manufacturer mf on mf.Id = md.ManufacturerID
                                 left join Engine e on e.Id = v.EngineId";
                
                var result = db.Query<VehicleDetails>(sqlQuery).ToList();

                return result;
            }
        }
    }

    public class GetSingleVehicleDetails : IQuery
    {
        public GetSingleVehicleDetails(Guid guid)
        {
            this.Guid = guid;
        }

        public Guid Guid { get; set; }
    }

    public class GetSingleVehicleDetailsHandler : IQueryHandler<GetSingleVehicleDetails, VehicleDetails>
    {
        public VehicleDetails Handle(GetSingleVehicleDetails query)
        {
            using (var db = new SqlConnection(@"Server=.;Database=FuelTracker;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var sqlQuery = @"select v.guid, mf.name as manufacturer, md.name as model, v.productionyear, e.name as enginename, e.power, e.torque, e.cylinders, e.displacement, e.fueltype
                                 from vehicle v
                                 join ModelName md on md.Id = v.ModelNameId
                                 left join Manufacturer mf on mf.Id = md.ManufacturerID
                                 left join Engine e on e.Id = v.EngineId
                                 where v.Guid = @Guid";


                var result = db.Query<VehicleDetails>(sqlQuery, new { Guid = query.Guid }).SingleOrDefault();

                return result;
            }
        }
    }

    public class VehicleDetails
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
