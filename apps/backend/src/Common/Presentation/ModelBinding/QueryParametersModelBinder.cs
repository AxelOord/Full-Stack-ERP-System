using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentation.ModelBinding;
public class QueryParametersModelBinder : IModelBinder
{
    private readonly Dictionary<string, string> _reservedParams = new()
    {
        { "pagenumber", "PageNumber" },
        { "pagesize", "PageSize" },
        { "filterproperty", "FilterProperty" },
        { "filtervalue", "FilterValue" },
        { "orderby", "OrderBy" },
        { "isdescending", "IsDescending" }
    };

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        // Create a new instance of GetAllQueryParameters
        var model = new GetAllQueryParameters();

        // Get the query string values
        var queryCollection = bindingContext.HttpContext.Request.Query;

        // Process reserved parameters (standard pagination and simple filtering)
        foreach (var param in queryCollection)
        {
            var key = param.Key.ToLowerInvariant();
            var value = param.Value.ToString();

            if (_reservedParams.TryGetValue(key, out var propName))
            {
                var property = typeof(GetAllQueryParameters).GetProperty(propName);

                if (property != null)
                {
                    // Convert the value to the appropriate type
                    object convertedValue;

                    if (property.PropertyType == typeof(int))
                    {
                        convertedValue = int.TryParse(value, out var intValue) ? intValue : 0;
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        convertedValue = bool.TryParse(value, out var boolValue) && boolValue;
                    }
                    else
                    {
                        convertedValue = value;
                    }

                    // Set the property value
                    property.SetValue(model, convertedValue);
                }
            }
            // Store all potential filter parameters for later processing
            else if (key.Contains("[") && key.Contains("]"))
            {
                model.RawFilters[param.Key] = value;
            }
        }

        // Set the result
        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}