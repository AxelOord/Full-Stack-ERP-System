# Common Patterns and Usage Guide

## CQRS Implementation

The system implements Command Query Responsibility Segregation (CQRS) pattern using MediatR.

### Commands

Commands represent operations that change state. They follow this pattern:

1. Define a command record:

```csharp
public sealed record CreateArticleCommand(CreateArticleRequest Request) : ICommand;
```

2. Implement a command handler:

```csharp
internal sealed class CreateArticleCommandHandler : ICommandHandler<CreateArticleCommand>
{
    // Dependencies via constructor injection

    public async Task<Result> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
    {
        // Implementation
        return Result.Success();
    }
}
```

### Queries

Queries represent operations that retrieve data without changing state:

1. Define a query record:

```csharp
public sealed record GetByIdQuery<T>(Guid Id) : IQuery<T> where T : IEntity;
```

2. Implement a query handler:

```csharp
internal sealed class GetByIdQueryHandler<T> : IQueryHandler<GetByIdQuery<T>, T>
    where T : IEntity
{
    // Dependencies via constructor injection
    
    public async Task<Result<T>> Handle(GetByIdQuery<T> request, CancellationToken cancellationToken)
    {
        // Implementation
        return await _service.ExecuteAsync(request, cancellationToken);
    }
}
```

## Using the Generic BaseController

The `BaseController` provides common CRUD operations for entities:

```csharp
public class ProductsController : BaseController<Product, ProductDto, ProductDbContext>
{
    public ProductsController(IMapper mapper, IMediator mediator, Func<Type, Type, Type, object> factory)
        : base(mapper, mediator, factory)
    {
    }
    
    // Inherit these endpoints:
    // GET /api/products/{id} - GetByIdAsync
    // GET /api/products - GetAllAsync
    // DELETE /api/products/{id} - DeleteAsync
    
    // Add custom endpoints:
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

## Result Pattern

The system uses a Result pattern to handle operation outcomes:

```csharp
// Success with no return value
return Result.Success();

// Success with return value
return Result.Success(entity);

// Failure with error
return Result.Failure(new NotFoundError(nameof(Entity)));
```

Converting results to HTTP responses:

```csharp
// In controllers
return result.ToActionResult();
// or with specific status code
return result.ToActionResult(StatusCodes.Status201Created);
```

## REST API Response Format

The system uses a standardized JSON:API-inspired response format:

### Single Resource Response

```json
{
  "data": [
    {
      "type": "article",
      "id": "f89b2837-7a6c-4b0e-8a1f-14a5b3372fa8",
      "attributes": {
        "articleNumber": "ABC123",
        "name": "Test Article",
        "isActive": true,
        "supplierName": "Test Supplier"
      },
      "links": {
        "self": "https://localhost:7013/api/articles/f89b2837-7a6c-4b0e-8a1f-14a5b3372fa8"
      }
    }
  ],
  "metadata": {
    "columns": [
      {
        "field": "articleNumber",
        "key": "COLUMN_NAME_ARTICLE_NUMBER",
        "type": "text",
        "sortable": true
      },
      {
        "field": "name",
        "key": "COLUMN_NAME_NAME",
        "type": "text",
        "sortable": false
      }
    ]
  }
}
```

### Collection Response

```json
{
  "links": {
    "self": {
      "href": "https://localhost:7013/api/articles?pageNumber=1&pageSize=10",
      "rel": "self",
      "method": "GET"
    },
    "next": {
      "href": "https://localhost:7013/api/articles?pageNumber=2&pageSize=10",
      "rel": "next",
      "method": "GET"
    }
  },
  "data": [...],
  "metadata": {...}
}
```

## Using Annotations

### Response Metadata Annotations

Apply these attributes to DTO properties to customize response metadata:

```csharp
public class ArticleDto : IDto
{
    [TranslationKey("COLUMN_NAME_SUPPLIER_NAME")]
    [Sortable]
    public required string SupplierName { get; set; }

    [Required]
    [TranslationKey("COLUMN_NAME_ARTICLE_NUMBER")]
    [Sortable]
    public required string ArticleNumber { get; set; }

    [Required]
    [TranslationKey("COLUMN_NAME_NAME")]
    [Sortable(false)]
    public required string Name { get; set; }

    [TranslationKey("COLUMN_NAME_IS_ACTIVE")]
    [Sortable]
    public bool IsActive { get; set; }
}
```

- `[TranslationKey]`: Provides a translation key for the field
- `[Sortable]`: Indicates if the field can be sorted (defaults to true)
- `[DataType]`: Specifies the data type (defaults to auto-detected)

### Other Common Annotations

- `[NullableProp]`: Indicates a nullable reference property in OpenAPI schemas
- `[Required]`: Marks a property as required (validation)