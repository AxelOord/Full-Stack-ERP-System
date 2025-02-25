using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Shared;
using System.Reflection;

namespace App;

public class NullableSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        foreach (var property in type.GetProperties())
        {
            if (property.GetCustomAttribute<NullableProp>() != null)
            {
                var schemaProperties = new Dictionary<string, OpenApiSchema>(schema.Properties, StringComparer.OrdinalIgnoreCase);

                schemaProperties.TryGetValue(property.Name, out var prop);

                if (prop != null && prop.Nullable && (string)prop.Annotations.First().Value == property.PropertyType.Name)
                {
                    Console.WriteLine($"Fixing nullable reference for: {property.Name}"); // We really need a debug logger / logger setup
                    prop.Nullable = true;
                    prop.Reference = new OpenApiReference { Id = property.PropertyType.Name, Type = ReferenceType.Schema };
                }
            }
        }

        return Task.CompletedTask;
    }
}
