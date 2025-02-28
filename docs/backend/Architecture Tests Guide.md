# Architecture Tests Guide

## Overview

The application includes architecture tests to enforce clean architecture principles and coding standards. These tests verify that the code structure adheres to the defined architectural constraints.

## Test Framework

The architecture tests use:

- xUnit: Testing framework
- NetArchTest: For testing architectural constraints
- FluentAssertions: For fluent assertion syntax

## Test Structure

Architecture tests are located in the `test/ArchitectureTests` project. They're organized by module:

```
test/ArchitectureTests/
├── BaseTest.cs                # Base class for all architecture tests
├── Common/                    # Common architecture tests
└── Modules/                   # Module-specific architecture tests
    └── Warehouse/             # Warehouse module tests
        ├── ApplicationTests.cs
        ├── DomainTests.cs
        └── LayerTests.cs
```

## Base Test Class

The `BaseTest` class sets up common components for architecture tests:

```csharp
public abstract class BaseTest
{
    // References to assemblies to test
    protected static readonly Assembly WarehouseDomainAssembly = 
        Warehouse.Domain.AssemblyReference.Assembly;
    protected static readonly Assembly WarehouseApplicationAssembly = 
        Warehouse.Application.AssemblyReference.Assembly;
    // ... other assemblies

    // Architecture definition for testing
    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            WarehouseDomainAssembly,
            WarehouseApplicationAssembly,
            // ... other assemblies
        )
        .Build();

    // Layer definitions for dependency tests
    protected static readonly IObjectProvider<IType> DomainLayer =
        ArchRuleDefinition.Types().That().ResideInAssembly(WarehouseDomainAssembly)
            .As("Warehouse Domain layer");
    
    // ... other layers
}
```

## Layer Dependency Tests

Tests that enforce correct dependencies between architectural layers:

```csharp
public class LayerTests : BaseTest
{
    [Fact]
    public void Domain_Should_NotHaveDependencyOnApplication()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(DomainLayer)
            .Should()
            .NotDependOnAny(ApplicationLayer)
            .Check(Architecture);
    }

    // Other layer dependency tests
}
```

## Naming Convention Tests

Tests that enforce naming conventions:

```csharp
public class ApplicationTests : BaseTest
{
    [Fact]
    public void CommandHandler_ShouldHave_NameEndingWith_CommandHandler()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .Check(Architecture);
    }

    // Other naming convention tests
}
```

## Class Structure Tests

Tests that enforce class design principles:

```csharp
public class DomainTests : BaseTest
{
    [Fact]
    public void Entities_Should_HavePrivateParameterLessConstructors()
    {
        IEnumerable<Class> entityTypes = ArchRuleDefinition
          .Classes()
          .That()
          .AreAssignableTo(typeof(Entity))
          .GetObjects(Architecture);

        var failingTypes = new List<Class>();
        foreach (Class entityType in entityTypes)
        {
            IEnumerable<MethodMember>? constructors = entityType.GetConstructors();

            if (!constructors.Any(c => 
                c.Visibility == Visibility.Private && !c.Parameters.Any()))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }

    // Other class structure tests
}
```

## Running Architecture Tests

To run the architecture tests:

```bash
# Run all architecture tests
dotnet test test/ArchitectureTests

# Run tests for a specific module
dotnet test --filter "FullyQualifiedName~ArchitectureTests.Modules.Warehouse"
```

## Adding New Architecture Tests

To add new architecture tests:

1. Create a new test class in the appropriate folder
2. Inherit from `BaseTest` to access common testing infrastructure
3. Add test methods with assertions using ArchUnitNET

Example:

```csharp
public class NewModuleTests : BaseTest
{
    [Fact]
    public void Services_Should_BeSealed()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .HaveNameEndingWith("Service")
            .Should()
            .BeSealed()
            .Check(Architecture);
    }
}
```

## Adding Tests for New Modules

When adding a new module:

1. Update `BaseTest.cs` to include the new module's assemblies
2. Add layer definitions for the new module
3. Create test classes for the new module
4. Add standard tests for layer dependencies, naming conventions, etc.

## Common Architecture Rules

The architecture tests enforce these common rules:

1. **Layer Dependencies**: Ensure dependency flow is correct (Domain ← Application ← Infrastructure/Persistence/Presentation)
2. **Naming Conventions**: Enforce naming standards (e.g., CommandHandlers end with "CommandHandler")
3. **Access Modifiers**: Ensure classes have appropriate visibility (e.g., CommandHandlers are internal)
4. **Class Design**: Enforce design principles (e.g., Entities are sealed and have private parameterless constructors)