using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the warehouse module domain service installer.
/// </summary>
internal class DomainServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Warehouse.Domain.AssemblyReference.Assembly);
    }
}
