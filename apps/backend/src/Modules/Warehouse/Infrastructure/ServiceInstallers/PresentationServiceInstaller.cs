using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ModelBinding;


namespace Warehouse.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the warehouse module presentation service installer.
/// </summary>
internal sealed class PresentationServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddControllers()
            .AddApplicationPart(Presentation.AssemblyReference.Assembly)
            .AddMvcOptions(options =>
            {
                options.ModelBinderProviders.Insert(0, new QueryParametersModelBinderProvider());
            });
}
