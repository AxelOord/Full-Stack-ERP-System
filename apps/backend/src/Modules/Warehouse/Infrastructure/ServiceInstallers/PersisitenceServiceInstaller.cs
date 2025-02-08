using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Options;
using Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;
using Warehouse.Persistence.Constants;

namespace Warehouse.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the warehouse module persistence service installer.
/// </summary>
internal sealed class PersistenceServiceInstaller : IServiceInstaller
{
  /// <inheritdoc />
  public void Install(IServiceCollection services, IConfiguration configuration) =>
      services
          .AddDbContext<WarehouseDbContext>((serviceProvider, options) =>
          {
            ConnectionStringOptions connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()!.Value;

            options.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schemas.Warehouse)); // FIXME: create Extension method, so that only schema needs to be specified
          });
}
