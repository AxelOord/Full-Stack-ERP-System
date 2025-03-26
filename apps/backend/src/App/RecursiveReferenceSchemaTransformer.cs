using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace App;

public class RecursiveReferenceSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        // Check if we're dealing with FilterGroup
        if (type.Name == "FilterGroup")
        {
            // Look for the groups property in the schema
            if (schema.Properties.TryGetValue("groups", out var groupsProperty) &&
                groupsProperty.Type == "array" &&
                groupsProperty.Items != null)
            {
                // Check if items property is trying to use a malformed reference
                if (groupsProperty.Items.Reference != null &&
                    groupsProperty.Items.Reference.ReferenceV3 != null &&
                    groupsProperty.Items.Reference.ReferenceV3.Contains("#/items"))
                {
                    Console.WriteLine("Fixing recursive reference for FilterGroup.groups");

                    // Replace with proper self-reference
                    groupsProperty.Items.Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = "FilterGroup"
                    };
                }
            }
        }

        return Task.CompletedTask;
    }
}