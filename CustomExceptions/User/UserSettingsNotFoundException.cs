using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.User
{
    public class UserSettingsNotFoundException : Exception, IBadReuestException
    {
        private const string message = "Settings for user with id: {0} not exists";

        public UserSettingsNotFoundException(Guid userId) : base(string.Format(message, userId))
        {

        }
    }
}
