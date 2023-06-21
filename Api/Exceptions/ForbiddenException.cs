using System.Net;

namespace Api.Exceptions;

public class ForbiddenException : ExceptionBase
{
    public ForbiddenException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.Forbidden;
    }
}