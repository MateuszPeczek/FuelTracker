using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.User
{
    public  class EmptyUserPasswordException : Exception, IBadRequestException
    {
        private const string message = "Empty user password";

        public EmptyUserPasswordException() : base(message)
        {

        }
    }
}
