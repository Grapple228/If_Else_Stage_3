using System.Net;

namespace Api.Exceptions;

public class UnauthorizedException : ExceptionBase
{
    public UnauthorizedException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}