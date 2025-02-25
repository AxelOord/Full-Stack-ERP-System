using AutoMapper;
using Domain.Primitives.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using Shared.Results.PaginatedResponse;

namespace Shared.Results.Response;

public interface IResultMapper<TSource, TDestination, TResponse>
    where TSource : class
    where TDestination : IDto
    where TResponse : IResponse<TDestination>
{
    TResponse Map(Result<TSource> result, IMapper mapper, HttpRequest request);
}

public class SingleEntityMapper<TSource, TDestination> : IResultMapper<TSource, TDestination, Response<TDestination>>
    where TDestination : IDto
    where TSource : class, IEntity
{
    public Response<TDestination> Map(Result<TSource> result, IMapper mapper, HttpRequest request)
    {
        var apiData = ApiData<TDestination>.CreateApiData<TSource, TDestination>(result.Value, mapper, request);
        var response = new Response<TDestination>
        {
            Data = [apiData],
            Metadata = MetadataGenerator.GenerateMetadata<TDestination>()
        };

        return response;
    }
}

public class CollectionEntityMapper<TSource, TDestination> : IResultMapper<List<TSource>, TDestination, PaginatedResponse<TDestination>>
    where TSource : class, IEntity
    where TDestination : IDto
{
    public PaginatedResponse<TDestination> Map(Result<List<TSource>> result, IMapper mapper, HttpRequest request)
    {
        var apiData = result.Value.Select(entity => ApiData<TDestination>.CreateApiData<TSource, TDestination>(entity, mapper, request)).ToList();

        var response = new PaginatedResponse<TDestination>
        {
            Links = request.CreatePaginationLinks(
              routeName: request.Path,
              resultCount: apiData.Count
          ),
            Data = apiData,
            Metadata = MetadataGenerator.GenerateMetadata<TDestination>()
        };

        return response;
    }
}
