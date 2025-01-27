using Askify.Shared.Auth.Middlewares;
using Askify.Shared.Results;
using Humanizer;
using Microsoft.Extensions.Logging;

namespace Askify.Shared.Exceptions;

internal class ExceptionMiddleware : IMiddleware
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
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(exception, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        var (statusCode, error) = exception switch
        {
            TokenExpiredException or InvalidTokenException => (StatusCodes.Status401Unauthorized,
                new Error(exception.GetType().Name.Underscore().Replace("_exception", string.Empty),
                    exception.Message)),
            AskifyException => (StatusCodes.Status400BadRequest,
                new Error(exception.GetType().Name.Underscore().Replace("_exception", string.Empty),
                    exception.Message)),
            _ => (StatusCodes.Status500InternalServerError, new Error("error", "There was an error."))
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }

    private record Error(string Code, string Reason);
}