using System;
using System.Net;

namespace Api.Exceptions;

public abstract class ExceptionBase : Exception
{
    public int StatusCode { get; set; }

    protected ExceptionBase(string message) : base(message)
    {
        
    }
}