﻿using Common.Interfaces;
using CustomExceptions.Vehicle;
using Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Queries.VehicleQueries
{
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

                if (result == null)
                    throw new VehicleNotFoundException(query.Id);
                else
                    return result;
            }
        }
    }
}