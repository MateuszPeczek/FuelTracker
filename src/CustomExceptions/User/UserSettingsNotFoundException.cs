using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.User
{
    public class UserSettingsNotFoundException : Exception, IBadRequestException
    {
        private const string message = "Settings for user with id: {0} not exists";

        public UserSettingsNotFoundException(Guid userId) : base(string.Format(message, userId))
        {

        }
    }
}
