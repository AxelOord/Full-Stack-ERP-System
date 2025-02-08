using Microsoft.EntityFrameworkCore;
using Warehouse.Persistence.Constants;

namespace Warehouse.Persistence;

/// <summary>
/// Represents the warehouse module database context.
/// </summary>
public sealed class WarehouseDbContext : DbContext
{
  /// <summary>
  /// Initializes a new instance of the <see cref="WarehouseDbContext"/> class.
  /// </summary>
  /// <param name="options">The database context options.</param>
  public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
      : base(options)
  {
  }

  /// <inheritdoc />
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema(Schemas.Warehouse);

    modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
  }
}

