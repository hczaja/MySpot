using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MySpot.Core.Exceptions;

namespace MySpot.Infrastructure.Exceptions;

internal sealed class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    
    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception  exception)
        {
            _logger.Log(LogLevel.Error, exception.Message);
            await HandleExceptionAsync(exception, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext httpContext)
    {
        var (statusCode, error) = exception switch
        {
            CustomException => (
                StatusCodes.Status400BadRequest, 
                new Error(exception.GetType().Name.Replace("Exception", string.Empty), exception.Message)),
            _ => (
                StatusCodes.Status500InternalServerError, 
                new Error("error", "There was an error."))
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(error);
    }

    private record Error(string Code, string Reason);
}
