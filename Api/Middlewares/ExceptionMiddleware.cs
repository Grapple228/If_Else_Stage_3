using System;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Api.Middlewares;


public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            if (error is ExceptionBase ex)
            {
                response.StatusCode = ex.StatusCode;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}