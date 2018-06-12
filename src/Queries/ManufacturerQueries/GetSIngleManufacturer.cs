using Common.Interfaces;
using CustomExceptions.Manufacturer;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace Queries.ManufacturerQueries
{
    public class GetSingleManufacturer : IQuery
    {
        public Guid Id { get; set; }

        public GetSingleManufacturer(Guid id)
        {
            Id = id;
        }
    }

    public class GetSingleManufacturerHandler : IQueryHandler<GetSingleManufacturer, ManufacturerDetails>
    {
        public ManufacturerDetails Handle(GetSingleManufacturer query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = @"SELECT Id, Name FROM Manufacturer WHERE Id = @id";

                var result = db.Query<ManufacturerDetails>(sqlQuery, new { Id = query.Id }).SingleOrDefault();

                if (result == null)
                    throw new ManufacturerNotFoundException(query.Id);
                else
                    return result;
            }
        }
    }
}
