using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.User
{
    public  class InvalidUserEmailException : Exception, IBadRequestException
    {
        private const string message = "Invalid user email";

        public InvalidUserEmailException() : base(message)
        {

        }
    }
}
