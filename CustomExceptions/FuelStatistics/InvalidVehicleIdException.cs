using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.FuelStatistics
{
    public class InvalidVehicleIdException : Exception, IBadReuestException
    {
        private const string message = "Vehicle id cannot by empty";

        public InvalidVehicleIdException() : base(message)
        {

        }
    }
}
