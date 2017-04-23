using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Vehicle
{
    public class InvalidVehicleIdException : Exception, IBadReuestException
    {
        private const string message = "Vehicle id cannot by empty";

        public InvalidVehicleIdException() : base(message)
        {

        }
    }
}
