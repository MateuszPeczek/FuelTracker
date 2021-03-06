﻿using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.User
{
    public class UserNotFoundException : Exception, INotFoundException
    {
        private const string message = "User with id: {0} not exists";

        public UserNotFoundException(Guid userId) : base(string.Format(message, userId))
        {

        }
    }
}
