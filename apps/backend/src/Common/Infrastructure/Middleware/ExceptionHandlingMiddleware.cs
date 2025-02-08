using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middleware
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An unhandled exception occurred.");
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var statusCode = exception switch
      {
        KeyNotFoundException => StatusCodes.Status404NotFound,
        NotImplementedException => StatusCodes.Status501NotImplemented,
        _ => StatusCodes.Status500InternalServerError
      };

      var problemDetails = new ProblemDetails
      {
        Status = statusCode,
        Title = "An error occurred while processing your request.",
        Detail = exception.Message,
        Instance = context.Request.Path
      };

      context.Response.ContentType = "application/problem+json";
      context.Response.StatusCode = statusCode;

      return context.Response.WriteAsJsonAsync(problemDetails);
    }
  }
}
