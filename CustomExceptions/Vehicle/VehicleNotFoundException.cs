using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Vehicle
{
    public class VehicleNotFoundException : Exception, INotFoundException
    {
        private const string message = "Vehicle with id: {0} not exist";

        public VehicleNotFoundException(Guid vehicleId) : base(string.Format(message, vehicleId))
        {

        }
    }
}
