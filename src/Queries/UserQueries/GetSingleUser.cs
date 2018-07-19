using Common.Interfaces;
using CustomExceptions.User;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace Queries.UserQueries
{
    public class GetSingleUser : IQuery
    {
        public Guid Id { get; set; }

        public GetSingleUser(Guid id)
        {
            Id = id;
        }
    }

    public class GetSingleUserHandler : IQueryHandler<GetSingleUser, UserDetails>
    {
        public UserDetails Handle(GetSingleUser query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = $@"SELECT Id, Email 
                                 FROM AspNetUsers
                                 WHERE ID = @Id";

                var result = db.Query<UserDetails>(sqlQuery, new { Id = query.Id }).SingleOrDefault();

                if (result == null)
                    throw new UserNotFoundException(query.Id);
                else
                    return result;
            }
        }
    }
}
