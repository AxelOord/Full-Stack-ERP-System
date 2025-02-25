using System.Reflection;

namespace Shared.Results.Response;

public static class MetadataGenerator
{
    public static Metadata GenerateMetadata<T>()
    {
        var metadata = new Metadata();
        var columns = new List<Column>();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
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

        metadata.Columns = columns;
        return metadata;
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
