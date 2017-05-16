using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Manufacturer
{
    public class ManufacturerNotFoundException : Exception, INotFoundException
    {
        private const string message = "Manufacturer with id: {0} not exists";

        public ManufacturerNotFoundException(Guid id) : base(string.Format(message, id))
        {

        }
    }
}
