using AutoMapper;
using Domain.Primitives.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Shared.Results.Response;

public class ApiData<T>
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("attributes")]
    public required T Attributes { get; set; }

    [JsonPropertyName("links")]
    public required AttributeLinks Links { get; set; }

    public static ApiData<T> CreateApiData<TSource>(TSource entity, IMapper mapper, HttpRequest request)
     where TSource : IEntity
    {
        var attributes = mapper.Map<T>(entity);

        CopyAndWrapNestedCollections(entity, attributes, mapper, request);

        return new ApiData<T>
        {
            Type = typeof(TSource).Name.ToLower(),
            Id = entity.Id,
            Attributes = attributes,
            Links = request.CreateDtoLinks(typeof(TSource).Name.ToLower(), entity.Id)
        };
    }

    private static void CopyAndWrapNestedCollections<TSource, TDestination>(
    TSource entity,
    TDestination dto,
    IMapper mapper,
    HttpRequest request)
    {
        var dtoProps = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var entityProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var dtoProp in dtoProps)
        {
            if (!dtoProp.CanWrite || !dtoProp.PropertyType.IsGenericType) continue;

            var dtoPropType = dtoProp.PropertyType.GetGenericTypeDefinition();
            if (dtoPropType != typeof(List<>)) continue;

            var dtoItemWrapperType = dtoProp.PropertyType.GetGenericArguments()[0];

            if (!dtoItemWrapperType.IsGenericType || dtoItemWrapperType.GetGenericTypeDefinition() != typeof(ApiData<>))
                continue;

            var innerDtoType = dtoItemWrapperType.GetGenericArguments()[0];

            var entityProp = entityProps.FirstOrDefault(p =>
                string.Equals(p.Name, dtoProp.Name, StringComparison.OrdinalIgnoreCase)
            );

            if (entityProp == null) continue;

            var entityList = entityProp.GetValue(entity) as IEnumerable;
            if (entityList == null) continue;

            var wrappedListType = typeof(List<>).MakeGenericType(dtoItemWrapperType);
            var wrappedList = (IList)Activator.CreateInstance(wrappedListType)!;

            foreach (var item in entityList)
            {
                var apiDataGenericType = typeof(ApiData<>).MakeGenericType(innerDtoType);
                var createMethod = apiDataGenericType.GetMethod(nameof(ApiData<object>.CreateApiData), BindingFlags.Public | BindingFlags.Static)!
                    .MakeGenericMethod(item.GetType());

                var wrapped = createMethod.Invoke(null, new object[] { item, mapper, request });
                wrappedList.Add(wrapped!);
            }

            dtoProp.SetValue(dto, wrappedList);
        }
    }
}
