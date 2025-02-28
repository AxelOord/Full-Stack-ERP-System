# Database Configuration Guide

## Entity Framework Core Configuration

The application uses Entity Framework Core with SQL Server. Each module has its own `DbContext` and database schema.

## Database Context Setup

Each module defines its own database context:

```csharp
public sealed class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Warehouse);
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
```

## Entity Configurations

Use the Entity Configuration pattern for configuring entity mapping:

```csharp
public class SupplierConfigurations : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SbnId).IsRequired();
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.IsActive).IsRequired();

        builder.HasIndex(s => s.SbnId)
          .IsUnique();

        builder.HasMany(s => s.Articles)
          .WithOne(a => a.Supplier)
          .HasForeignKey(a => a.SupplierId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}
```

## Database Schema Configuration

Each module uses its own database schema:

```csharp
// Define schema constants
public static class Schemas
{
    public const string Warehouse = "warehouse";
    // Other schemas for other modules
}

// Use in DbContext
modelBuilder.HasDefaultSchema(Schemas.Warehouse);
```

## Connection String Configuration

The connection string is managed in `appsettings.json` and injected via options pattern:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=db,1433;Database=JeeoV2;User Id=sa;Password=Password123;Trust Server Certificate=True"
  }
}
```

## Database Registration

Each module's database context is registered in its module installer:

```csharp
internal sealed class PersistenceServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<ModuleDbContext>((serviceProvider, options) =>
            {
                ConnectionStringOptions connectionString = serviceProvider
                    .GetService<IOptions<ConnectionStringOptions>>()!.Value;

                options.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory", 
                        Schemas.ModuleName));
            });
}
```

## Migrations

To create and apply migrations for a module:

```bash
# Create a migration
dotnet ef migrations add InitialMigration --project src/Modules/ModuleName/Persistence --startup-project src/App

# Apply migrations
dotnet ef database update --project src/Modules/ModuleName/Persistence --startup-project src/App
```

## Automatic Migrations on Startup

The system includes a `MigrateDatabaseStartupTask` that automatically applies migrations in development:

```csharp
internal sealed class MigrateDatabaseStartupTask : BackgroundService
{
    // Dependencies via constructor injection
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_environment.IsDevelopment())
        {
            return;
        }

        using IServiceScope scope = _serviceProvider.CreateScope();
        await MigrateDatabaseAsync<ModuleDbContext>(scope, stoppingToken);
    }
    
    private static async Task MigrateDatabaseAsync<TDbContext>(
        IServiceScope scope, 
        CancellationToken cancellationToken)
        where TDbContext : DbContext
    {
        try
        {
            TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            
            if (!await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                await dbContext.Database.EnsureCreatedAsync(cancellationToken);
            }
            
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database migration failed: {ex.Message}");
        }
    }
}
```

## Docker Database Setup

For local development, the database is available in the Docker Compose setup:

```yaml
services:
  app:
    # Application configuration
    depends_on:
      - db
      
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    ports:
      - "1433:1433"
    environment:
      - SA_PASSWORD=Password123
      - ACCEPT_EULA=Y
    volumes:
      - db-data:/var/opt/mssql
    networks:
      - app-network

volumes:
  db-data:
```