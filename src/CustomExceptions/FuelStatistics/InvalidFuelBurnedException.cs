using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.FuelStatistics
{
    public class InvalidFuelBurnedException : Exception, IBadRequestException
    {
        private const string message = "Burned fuel amount has to be greater than 0";

        public InvalidFuelBurnedException() : base(message)
        {

        }
    }
}
