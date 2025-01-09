using AutoMapper;
using Domain.Primitives;
using Microsoft.AspNetCore.Http;

namespace Domain.Shared.ApiResponse
{
    public interface IResultMapper<TSource, TDestination>
        where TSource : class
    {
        List<ApiData<TDestination>> Map(Result<TSource> result, IMapper mapper, LinkBuilder linkBuilder, HttpRequest request);
    }

    public class SingleEntityMapper<TSource, TDestination> : IResultMapper<TSource, TDestination>
        where TDestination : Dto
        where TSource : Entity
    {
        public List<ApiData<TDestination>> Map(Result<TSource> result, IMapper mapper, LinkBuilder linkBuilder, HttpRequest request)
        {
            var apiData = ApiData<TDestination>.CreateApiData<TSource, TDestination>(result.Value, mapper, linkBuilder, request);
            return [apiData];
        }
    }

    public class CollectionEntityMapper<TSource, TDestination> : IResultMapper<List<TSource>, TDestination>
        where TSource : Entity
    {
        public List<ApiData<TDestination>> Map(Result<List<TSource>> result, IMapper mapper, LinkBuilder linkBuilder, HttpRequest request)
        {
            return result.Value.Select(entity => ApiData<TDestination>.CreateApiData<TSource, TDestination>(entity, mapper, linkBuilder, request)).ToList();
        }
    }
}
