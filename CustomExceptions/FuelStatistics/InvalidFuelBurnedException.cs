using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.FuelStatistics
{
    public class InvalidFuelBurnedException : Exception, IBadReuestException
    {
        private const string message = "Burned fuel amount has to be greater than 0";

        public InvalidFuelBurnedException() : base(message)
        {

        }
    }
}
