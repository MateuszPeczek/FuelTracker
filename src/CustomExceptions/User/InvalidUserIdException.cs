using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.User
{
   public  class InvalidUserIdException : Exception, IBadRequestException
    {
        private const string message = "Invalid User id";

        public InvalidUserIdException() : base(message)
        {

        }
    }
}
