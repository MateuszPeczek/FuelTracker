using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Model
{
    public class ModelNotFoundException : Exception, INotFoundException
    {
        private const string message = "Model with id: {0} not exists";

        public ModelNotFoundException(Guid id) : base(string.Format(message, id))
        {

        }
    }
}
