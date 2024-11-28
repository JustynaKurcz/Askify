using Askify.Shared.Results;

namespace Askify.Shared.Validation;

public class ValidationExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => HandleValidationExceptionAsync(context, validationException),
            _ => HandleUnexpectedExceptionAsync(context, exception)
        };
    }


    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = 420;
        context.Response.ContentType = "application/json";

        var errorResponse = exception.Errors
            .Select(e => new Error(e.ErrorCode, e.ErrorMessage))
            .First();

        var jsonResponse = JsonSerializer.Serialize(errorResponse);

        return context.Response.WriteAsync(jsonResponse);
    }

    private static Task HandleUnexpectedExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            Code = 500,
            Message = "There was an error"
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse);
        return context.Response.WriteAsync(jsonResponse);
    }
}