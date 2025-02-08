using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Warehouse.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the users module application service installer.
/// </summary>
internal sealed class ApplicationServiceInstaller : IServiceInstaller
{
  /// <inheritdoc />
  public void Install(IServiceCollection services, IConfiguration configuration) =>
      services
          .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));
}
