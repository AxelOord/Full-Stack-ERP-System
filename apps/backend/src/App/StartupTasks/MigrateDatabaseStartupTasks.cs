using Microsoft.EntityFrameworkCore;
using Warehouse.Persistence;

namespace App.StartupTasks;

/// <summary>
/// Represents the startup task for migrating the database in the development environment only.
/// </summary>
internal sealed class MigrateDatabaseStartupTask : BackgroundService
{
    private readonly IHostEnvironment _environment;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrateDatabaseStartupTask"/> class.
    /// </summary>
    /// <param name="environment">The environment.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public MigrateDatabaseStartupTask(IHostEnvironment environment, IServiceProvider serviceProvider)
    {
        _environment = environment;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_environment.IsDevelopment())
        {
            return;
        }

        using IServiceScope scope = _serviceProvider.CreateScope();

        await MigrateDatabaseAsync<WarehouseDbContext>(scope, stoppingToken);
    }

    private static async Task MigrateDatabaseAsync<TDbContext>(IServiceScope scope, CancellationToken cancellationToken)
        where TDbContext : DbContext
    {
        try
        {
            TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

            // Check if the database exists before applying migrations
            if (!await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                await dbContext.Database.EnsureCreatedAsync(cancellationToken);
            }

            // Apply migrations only if the database already exists
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database migration failed: {ex.Message}");
        }
    }

}
