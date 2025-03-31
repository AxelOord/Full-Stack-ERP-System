using System.Collections;
using System.Reflection;

namespace Shared.Results.Response;

public static class MetadataGenerator
{
    public static Metadata GenerateMetadata<T>()
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var metadata = new Metadata
        {
            Columns = GetColumns(properties.Where(p => p.GetCustomAttribute<ExpandableAttribute>() == null).ToArray()),
            Expandable = GetExpandableMetadata(properties)
        };

        return metadata;
    }

    private static Dictionary<string, Metadata> GetExpandableMetadata(PropertyInfo[] properties)
    {
        var expandableMetadata = new Dictionary<string, Metadata>();

        foreach (var prop in properties.Where(p => p.GetCustomAttribute<ExpandableAttribute>() != null))
        {
            Type nestedType = prop.PropertyType;

            if (typeof(IEnumerable).IsAssignableFrom(nestedType) && nestedType.IsGenericType)
            {
                Type itemType = nestedType.GetGenericArguments()[0];

                if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(ApiData<>))
                {
                    nestedType = itemType.GetGenericArguments()[0];
                }
                else
                {
                    nestedType = itemType;
                }
            }

            else if (nestedType.IsGenericType && nestedType.GetGenericTypeDefinition() == typeof(ApiData<>))
            {
                nestedType = nestedType.GetGenericArguments()[0];
            }

            var nestedProperties = nestedType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var nestedColumns = GetColumns(nestedProperties);

            string fieldName = $"{char.ToLowerInvariant(prop.Name[0])}{prop.Name[1..]}";
            expandableMetadata[fieldName] = new Metadata { Columns = nestedColumns };
        }

        return expandableMetadata;
    }

    private static List<Column> GetColumns(PropertyInfo[] properties)
    {
        var columns = new List<Column>();

        foreach (var prop in properties)
        {
            var filterPathAttr = prop.GetCustomAttribute<FilterPathAttribute>();
            var column = new Column
            {
                Field = $"{char.ToLowerInvariant(prop.Name[0])}{prop.Name[1..]}"
            };

            // Label
            var labelAttr = prop.GetCustomAttribute<TranslationKeyAttribute>();
            column.Key = labelAttr?.Value ?? Capitalize(prop.Name);

            // Data type
            var dataTypeAttr = prop.GetCustomAttribute<DataTypeAttribute>();
            column.Type = dataTypeAttr?.Value ?? DetermineType(prop.PropertyType);

            // Sortable
            var sortableAttr = prop.GetCustomAttribute<SortableAttribute>();
            column.Sortable = sortableAttr?.Value ?? true;

            columns.Add(column);
        }

        return columns;
    }

    private static string DetermineType(Type propertyType)
    {
        if (IsNumericType(propertyType))
        {
            return "number";
        }
        if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
        {
            return "date";
        }

        return "text";
    }

    private static bool IsNumericType(Type type)
    {
        return type == typeof(int) || type == typeof(long) || type == typeof(double) ||
               type == typeof(float) || type == typeof(decimal) ||
               type == typeof(int?) || type == typeof(long?) ||
               type == typeof(double?) || type == typeof(float?) || type == typeof(decimal?);
    }

    private static string Capitalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return char.ToUpper(s[0]) + s[1..];
    }
}
