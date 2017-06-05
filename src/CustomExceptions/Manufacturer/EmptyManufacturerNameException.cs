using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Manufacturer
{
    public class EmptyManufacturerNameException : Exception, IBadRequestException
    {
        private const string message = "Manufacturer name cannot be empty";

        public EmptyManufacturerNameException() : base(message)
        {
        }
    }
}
