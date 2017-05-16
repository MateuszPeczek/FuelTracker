﻿using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.Manufacturer
{
    public class InvalidManufacturerIdException : Exception, IBadReuestException
    {
        private const string message = "Invalid manudfacturer id";

        public InvalidManufacturerIdException() : base(message)
        {
        }
    }
}