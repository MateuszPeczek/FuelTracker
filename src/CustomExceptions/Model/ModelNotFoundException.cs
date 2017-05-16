using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Model
{
    public class ModelNotFoundException : Exception, INotFoundException
    {
        private const string message = "Model with id: {1} not exists in manufactuers with id: {0}";

        public ModelNotFoundException(Guid manufactuerId, Guid modelId) : base(string.Format(message, manufactuerId, modelId))
        {

        }
    }
}
