using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.User
{
    public class EmptyUserFirstNameException : Exception, INotFoundException
    {
        private const string message = "empty user fisrt name";

        public EmptyUserFirstNameException() : base(message)
        {
        }
    }
}
