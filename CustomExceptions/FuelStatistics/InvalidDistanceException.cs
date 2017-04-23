using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.FuelStatistics
{
    public class InvalidDistanceException : Exception, IBadReuestException
    {
        private const string message = "Distance has to be greater than 0";

        public InvalidDistanceException() : base(message)
        {

        }
    }
}
