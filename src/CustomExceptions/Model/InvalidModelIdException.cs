using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Model
{
    public class InvalidModelIdException : Exception, IBadReuestException
    {
        private const string message = "Invalid model id";

        public InvalidModelIdException() : base(message)
        {

        }
    }
}
