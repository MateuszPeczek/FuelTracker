using CustomExceptions.GroupingIntefaces;
using System;

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
