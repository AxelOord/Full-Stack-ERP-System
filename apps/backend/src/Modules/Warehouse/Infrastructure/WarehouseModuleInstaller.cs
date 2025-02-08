using Infrastructure.Configurations;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Infrastructure;

/// <summary>
/// Represents the warehouse module installer.
/// </summary>
public sealed class WarehouseModuleInstaller : IModuleInstaller
{
  /// <inheritdoc />
  public void Install(IServiceCollection services, IConfiguration configuration) =>
      services
          .InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly)
          //.AddTransientAsMatchingInterfaces(AssemblyReference.Assembly)
          //.AddTransientAsMatchingInterfaces(Persistence.AssemblyReference.Assembly)
          .AddScopedAsMatchingInterfaces(AssemblyReference.Assembly)
          .AddScopedAsMatchingInterfaces(Persistence.AssemblyReference.Assembly);
}

