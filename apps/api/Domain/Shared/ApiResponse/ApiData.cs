using AutoMapper;
using Domain.Primitives;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Domain.Shared.ApiResponse
{
    public class ApiData<T>
    {
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("attributes")]
        public required T Attributes { get; set; }

        [JsonPropertyName("links")]
        public ApiLinks? Links { get; set; }

        public static ApiData<TDestination> CreateApiData<TSource, TDestination>(TSource entity, IMapper mapper, LinkBuilder linkBuilder, HttpRequest request)
         where TSource : Entity
        {
            return new ApiData<TDestination>
            {
                Type = typeof(TSource).Name.ToLower(),
                Id = entity.Id,
                Attributes = mapper.Map<TDestination>(entity),
                Links = linkBuilder.CreateDtoLinks(request, typeof(TSource).Name.ToLower(), entity.Id)
            };
        }
    }
}
