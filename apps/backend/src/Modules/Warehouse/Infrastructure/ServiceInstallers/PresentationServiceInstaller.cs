using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Modules.Users.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the warehouse module presentation service installer.
/// </summary>
internal sealed class PresentationServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddControllers()
            .AddApplicationPart(Warehouse.Presentation.AssemblyReference.Assembly);
}
