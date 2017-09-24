using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Model
{
    public class InvalidModelIdException : Exception, IBadRequestException
    {
        private const string message = "Invalid model id";

        public InvalidModelIdException() : base(message)
        {

        }
    }
}
