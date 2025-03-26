using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentation.ModelBinding;
public class QueryParametersModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata.ModelType == typeof(GetAllQueryParameters))
        {
            return new QueryParametersModelBinder();
        }

        return null;
    }
}
