using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Manufacturer
{
    public class InvalidManufacturerIdException : Exception, IBadRequestException
    {
        private const string message = "Invalid manudfacturer id";

        public InvalidManufacturerIdException() : base(message)
        {
        }
    }
}
