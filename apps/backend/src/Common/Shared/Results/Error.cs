using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Results
{
  public abstract record Error
  {
    private readonly string _code;
    private readonly ProblemDetails _problemDetails;

    protected Error(string code, string message, int statusCode, string title, string type)
    {
      _code = code;
      _problemDetails = new ProblemDetails
      {
        Status = statusCode,
        Title = title,
        Type = type,
        Detail = message
      };
    }

    public int? GetStatusCode()
    {
      return _problemDetails.Status;
    }

    public ProblemDetails GetProblemDetails()
    {
      _problemDetails.Extensions["errorCode"] = _code;
      return _problemDetails;
    }
  }

  public record NotFoundError(string EntityName)
      : Error(
          code: "Error.NotFound",
          message: $"The {EntityName} was not found.",
          statusCode: StatusCodes.Status404NotFound,
          title: "Resource Not Found",
          type: "https://example.com/errors/not-found");

  public record NullValueError()
      : Error(
          code: "Error.NullValue",
          message: "The specified result value is null.",
          statusCode: StatusCodes.Status400BadRequest,
          title: "Invalid Input",
          type: "https://example.com/errors/null-value");

  public record InternalServerError(string Details)
      : Error(
          code: "Error.InternalServer",
          message: Details,
          statusCode: StatusCodes.Status500InternalServerError,
          title: "Internal Server Error",
          type: "https://example.com/errors/internal-server-error");

  public record ValidationError(string details)
          : Error(
              code: "Error.ValidationError",
              message: details,
              statusCode: StatusCodes.Status400BadRequest,
              title: "Validation Error",
              type: "https://example.com/errors/validation");
}
