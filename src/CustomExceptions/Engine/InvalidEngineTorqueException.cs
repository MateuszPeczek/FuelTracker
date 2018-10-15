using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Engine
{
    public class InvalidEngineTorqueException : Exception, IBadRequestException
    {
        private const string message = "Engine cannot have negative torque";

        public InvalidEngineTorqueException() : base(message)
        {

        }
    }
}
