using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Vehicle
{
    public class InvalidVehicleIdException : Exception, IBadReuestException
    {

    }
}
