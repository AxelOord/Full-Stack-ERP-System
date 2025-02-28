# Dependency Injection Guide

## DI Architecture

The application uses a hybrid approach to dependency injection:

1. **Microsoft.Extensions.DependencyInjection**: Primary DI container
2. **Autofac**: Used for advanced scenarios (like generic type resolution)
3. **Scrutor**: For scanning and registering services by convention

## Service Registration

### Service Installers

Service registration is organized using installers:

```csharp
public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
```

Implement this interface to create a service installer:

```csharp
internal sealed class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));
}
```

### Module Installers

For registering entire modules:

```csharp
public interface IModuleInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}

public sealed class WarehouseModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(Persistence.AssemblyReference.Assembly);
}
```

## Service Registration by Convention

The system uses service lifetime interfaces to register services by convention:

```csharp
// Interface mark for scoped services
public interface IScoped { }

// Example service implementation
public sealed class SupplierRepository : ISupplierRepository, IScoped
{
    // Implementation
}

// Register all IScoped services
services.AddScopedAsMatchingInterfaces(AssemblyReference.Assembly);
```

## Automatic Service Discovery

Services are discovered automatically using assembly scanning:

```csharp
// In Program.cs
builder.Services
    .InstallServicesFromAssemblies(
        builder.Configuration,
        App.AssemblyReference.Assembly,
        Persistence.AssemblyReference.Assembly)
    .InstallModulesFromAssemblies(
        builder.Configuration,
        Warehouse.Infrastructure.AssemblyReference.Assembly);
```

The extension method scans the assemblies for installer implementations:

```csharp
public static IServiceCollection InstallServicesFromAssemblies(
    this IServiceCollection services,
    IConfiguration configuration,
    params Assembly[] assemblies) =>
    services.Tap(
        () => InstanceFactory
            .CreateFromAssemblies<IServiceInstaller>(assemblies)
            .ForEach(serviceInstaller => 
                serviceInstaller.Install(services, configuration)));
```

## Generic Service Registration with Autofac

For generic services (like repositories), the application uses Autofac's advanced features:

```csharp
// Configure Autofac container
public static void ConfigureAutofacContainer(this ContainerBuilder containerBuilder)
{
    // Register factory for resolving generic services
    containerBuilder.Register<Func<Type, Type, Type, object>>(c =>
    {
        var ctxFactory = c.Resolve<IComponentContext>();
        return (entityType, serviceType, DbContextType) =>
        {
            var dbContext = ctxFactory.Resolve(DbContextType);

            var resolvedServiceType = serviceType switch
            {
                Type t when t == typeof(IGetByIdService<>) => 
                    typeof(GetByIdService<,>).MakeGenericType(DbContextType, entityType),
                Type t when t == typeof(IGetAllService<>) => 
                    typeof(GetAllService<,>).MakeGenericType(DbContextType, entityType),
                Type t when t == typeof(IDeleteService<>) => 
                    typeof(DeleteService<,>).MakeGenericType(DbContextType, entityType),
                _ => throw new InvalidOperationException("Unknown service type")
            };

            return Activator.CreateInstance(resolvedServiceType, dbContext);
        };
    });
}
```

This factory allows resolving generic services at runtime.

## Using Injected Services

In controllers, injected services are used with constructor injection:

```csharp
public class SuppliersController : BaseController<Supplier, SupplierDto, WarehouseDbContext>
{
    public SuppliersController(
        IMapper mapper, 
        IMediator mediator, 
        Func<Type, Type, Type, object> factory)
        : base(mapper, mediator, factory)
    {
    }
    
    // Controller actions use injected services
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateSupplierRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new CreateSupplierCommand(request);
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.ToActionResult(StatusCodes.Status201Created);
    }
}
```

## Service Lifetimes

Choose the appropriate service lifetime:

- **Scoped** (IScoped): Services that maintain state for a single request
- **Transient**: Services that are created each time they're requested
- **Singleton**: Services that are created once for the entire application

Most services in the application use Scoped lifetime.