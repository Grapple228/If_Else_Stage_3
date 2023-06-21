using System.Net;

namespace Api.Exceptions;

public class NotFoundException : ExceptionBase
{
    public NotFoundException(string message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }
}