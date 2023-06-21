using System.Net;

namespace Api.Exceptions;

public class BadRequestException : ExceptionBase
{
    public BadRequestException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
    }
}