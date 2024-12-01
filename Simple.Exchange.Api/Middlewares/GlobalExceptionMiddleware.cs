
using FluentValidation;

namespace Simple.Exchange.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionsAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionsAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = "application/json";

        // can return status code based on other exception types if required
        string message;
        if (ex is ValidationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            message = ex.Message;

        }
        else
        {
            message = "An error occured. Please try again.";
        }

        var result = new { Message = message };

        await httpContext.Response.WriteAsJsonAsync(result);
    }
}
