using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Model
{
    public class EmptyModelNameException : Exception, IBadRequestException
    {
        private const string message = "Model name cannot be empty";

        public EmptyModelNameException() : base(message)
        {

        }
    }
}
