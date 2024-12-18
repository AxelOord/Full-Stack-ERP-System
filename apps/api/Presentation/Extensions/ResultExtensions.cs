using Microsoft.AspNetCore.Mvc;
using Domain.Shared;
using AutoMapper;
using Domain.Shared.ApiResponse;

namespace Api.Extensions
{
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

        public static ActionResult<Response<TDestination>> ToActionResult<TSource, TDestination>(
            this Result<TSource> result,
            IMapper mapper,
            IResultMapper<TSource, TDestination> mapperStrategy,
            LinkBuilder linkBuilder,
            HttpRequest req)
            where TSource : class
        {
            if (!result.IsSuccess)
            {
                var error = result.Error!;
                return new ObjectResult(error.GetProblemDetails()) { StatusCode = error.GetStatusCode() };
            }

            var apiDataList = mapperStrategy.Map(result, mapper, linkBuilder, req);

            var response = new Response<TDestination>
            {
                Links = linkBuilder?.CreatePaginationLinks(
                    routeName: req.Path,
                    request: req,
                    resultCount: apiDataList.Count
                ),
                Data = apiDataList,
                Metadata = MetadataGenerator.GenerateMetadata<TDestination>()
            };

            return new ActionResult<Response<TDestination>>(response);
        }
    }
}