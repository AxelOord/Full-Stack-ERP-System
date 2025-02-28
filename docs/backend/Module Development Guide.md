# Module Development Guide

> **Warning**: This guide will be changing soon as much of the process will be automated using nx.

## Creating a New Module
To add a new module to the system, follow these steps:

1. Create the module directory structure:

```
src/Modules/{NewModule}/
├── Application/
├── Domain/
├── Infrastructure/
├── Persistence/
└── Presentation/
```

2. Add module projects to the solution:

```bash
dotnet new classlib -n {NewModule}.Domain -o src/Modules/{NewModule}/Domain
dotnet new classlib -n {NewModule}.Application -o src/Modules/{NewModule}/Application
dotnet new classlib -n {NewModule}.Infrastructure -o src/Modules/{NewModule}/Infrastructure
dotnet new classlib -n {NewModule}.Persistence -o src/Modules/{NewModule}/Persistence
dotnet new classlib -n {NewModule}.Presentation -o src/Modules/{NewModule}/Presentation
```

3. Add projects to the solution:

```bash
dotnet sln add src/Modules/{NewModule}/Domain/{NewModule}.Domain.csproj
dotnet sln add src/Modules/{NewModule}/Application/{NewModule}.Application.csproj
dotnet sln add src/Modules/{NewModule}/Infrastructure/{NewModule}.Infrastructure.csproj
dotnet sln add src/Modules/{NewModule}/Persistence/{NewModule}.Persistence.csproj
dotnet sln add src/Modules/{NewModule}/Presentation/{NewModule}.Presentation.csproj
```

4. Set up project references (each project should only reference what it needs according to clean architecture principles)

## Module Structure Requirements

### Domain Layer

- Create entity classes inheriting from `Entity` base class
- Define DTOs in the `Dto` namespace
- Add mapping profiles for AutoMapper
- Follow naming conventions for entities

```csharp
// Example entity
public sealed class Product : Entity
{
    // Properties with private setters
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    // Private constructor for EF Core
    private Product() { }
    
    // Private Constructor for creating new entities
    private Product(Guid id, string name, decimal price)
        : base(id)
    {
        Name = name;
        Price = price;
    }
    
    // Factory method
    public static Product Create(CreateProductRequest request)
    {
        return new Product(Guid.NewGuid(), request.Name, request.Price);
    }
}
```

### Application Layer

- Define commands and queries
- Implement command and query handlers
- Define service interfaces

```csharp
// Command
public sealed record CreateProductCommand(CreateProductRequest Request) : ICommand;

// Command Handler
internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly ICreateProductService _createProductService;
    
    public CreateProductCommandHandler(ICreateProductService createProductService)
    {
        _createProductService = createProductService;
    }
    
    public async Task<Result> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(command.Request);
        return await _createProductService.ExecuteAsync(product, cancellationToken);
    }
}
```

### Persistence Layer

- Implement repositories and services
- Add database configurations
- Define database context

```csharp
// Repository implementation
public class ProductRepository : IProductRepository, IScoped
{
    private readonly ModuleDbContext _context;
    
    public ProductRepository(ModuleDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<Product>()
                  .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
```

### Presentation Layer

- Create controllers inheriting from `BaseController`
- Implement API endpoints

```csharp
public class ProductsController : BaseController<Product, ProductDto, ModuleDbContext>
{
    public ProductsController(IMapper mapper, IMediator mediator, Func<Type, Type, Type, object> factory)
        : base(mapper, mediator, factory)
    {
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request);
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.ToActionResult(StatusCodes.Status201Created);
    }
}
```

### Infrastructure Layer

- Create module installer
- Set up service registrations

```csharp
public sealed class ModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(Persistence.AssemblyReference.Assembly);
}
```

## Register the New Module

Add the module to the App's Program.cs:

```csharp
builder.Services
        .InstallServicesFromAssemblies(
            builder.Configuration,
            App.AssemblyReference.Assembly,
            Persistence.AssemblyReference.Assembly)
        .InstallModulesFromAssemblies(
            builder.Configuration,
            Warehouse.Infrastructure.AssemblyReference.Assembly,
            NewModule.Infrastructure.AssemblyReference.Assembly); // Add this line
```