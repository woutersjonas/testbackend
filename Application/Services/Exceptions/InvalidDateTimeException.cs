﻿namespace jonas.Application.Services.Exceptions;

public class InvalidDateTimeException : Exception
{
    public InvalidDateTimeException(string message) : base(message) { }
}
