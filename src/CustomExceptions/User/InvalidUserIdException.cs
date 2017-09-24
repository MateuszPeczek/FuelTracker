using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.User
{
    public  class InvalidUserIdException : Exception, IBadRequestException
    {
        private const string message = "Invalid User id";

        public InvalidUserIdException() : base(message)
        {

        }
    }
}
