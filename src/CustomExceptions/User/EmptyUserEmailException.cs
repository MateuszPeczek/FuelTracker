using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.User
{
    public  class EmptyUserEmailException : Exception, IBadRequestException
    {
        private const string message = "Empty user email";

        public EmptyUserEmailException() : base(message)
        {

        }
    }
}
