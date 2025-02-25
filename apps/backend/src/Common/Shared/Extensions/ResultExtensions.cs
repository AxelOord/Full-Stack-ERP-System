using AutoMapper;
using Domain.Primitives.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using Shared.Results.Response;

namespace Shared.Extensions;

public static class ResultExtensions
{

    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new StatusCodeResult(204);
        }

        var error = result.Error!;

        return new ObjectResult(error.GetProblemDetails()) { StatusCode = error.GetStatusCode() };
    }

    public static ActionResult ToActionResult(this Result result, int status)
    {
        if (result.IsSuccess)
        {
            return new StatusCodeResult(status);
        }

        var error = result.Error!;

        return new ObjectResult(error.GetProblemDetails()) { StatusCode = error.GetStatusCode() };
    }

    public static ActionResult<TResponse> ToActionResult<TSource, TDestination, TResponse>(
        this Result<TSource> result,
        IMapper mapper,
        IResultMapper<TSource, TDestination, TResponse> mapperStrategy,
        HttpRequest req)
        where TSource : class
        where TDestination : IDto
        where TResponse : class, IResponse<TDestination>
    {
        if (!result.IsSuccess)
        {
            var error = result.Error!;
            return new ObjectResult(error.GetProblemDetails()) { StatusCode = error.GetStatusCode() };
        }

        var response = mapperStrategy.Map(result, mapper, req);

        return new ActionResult<TResponse>(response);
    }
}
