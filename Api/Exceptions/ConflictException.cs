using System.Net;

namespace Api.Exceptions;

public class ConflictException : ExceptionBase
{

    public ConflictException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.Conflict;
    }
}