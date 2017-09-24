using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Engine
{
    public class InvalidEngineIdException : Exception, IBadRequestException
    {
    }
}
