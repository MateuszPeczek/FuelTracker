using Common.Interfaces;
using CustomExceptions.User;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;

namespace Queries.UserQueries
{
    public class GetUserSettings : IQuery
    {
        public Guid Id { get; set; }

        public GetUserSettings(Guid id)
        {
            Id = id;
        }
    }

    public class GetUserSettingsHandler : IQueryHandler<GetUserSettings, Settings>
    {
        public Settings Handle(GetUserSettings query)
        {
            using (var db = new SqliteConnection($"Data Source=fueltracker.db"))
            {
                var sqlQuery = $@"SELECT Units 
                                 FROM UserSettings
                                 WHERE UserId = @Id";

                var result = db.Query<Settings>(sqlQuery, new { Id = query.Id }).SingleOrDefault();

                if (result == null)
                    throw new UserNotFoundException(query.Id);
                else
                    return result;
            }
        }
    }
}
