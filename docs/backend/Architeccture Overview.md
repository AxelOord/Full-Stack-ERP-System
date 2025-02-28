# Architecture Overview

## System Structure

The application follows a modular, clean architecture with distinct separation of concerns:

```
backend/
├── src/
│   ├── App/                    # Main application entry point
│   ├── Common/                 # Shared infrastructure, domain, etc.
│   └── Modules/                # Business modules (e.g., Warehouse)
│       └── {ModuleName}/       # Each module follows the same structure
│           ├── Application/    # Use cases, commands, queries
│           ├── Domain/         # Entities, value objects
│           ├── Infrastructure/ # Technical concerns
│           ├── Persistence/    # Data access
│           └── Presentation/   # API endpoints
└── test/
    └── ArchitectureTests/      # Architecture validation tests
```

## Key Design Principles

1. **Modularity**: Each business domain is contained in its own module
2. **Clean Architecture**: Dependencies point inward (Domain ← Application ← Infrastructure/Persistence/Presentation)
3. **CQRS**: Commands and Queries are separated for clarity
4. **Mediator Pattern**: Commands/Queries are dispatched through MediatR
5. **Generic Repository Pattern**: Common CRUD operations are abstracted

## Module Structure

Each module follows a consistent vertical slice architecture:

- **Domain**: Core business entities and logic
- **Application**: Use cases, commands, and queries
- **Persistence**: Database access and repositories
- **Presentation**: API controllers
- **Infrastructure**: Technical concerns and service registration

## Dependencies Between Layers

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   Presentation  │ ──► │   Application   │ ◄── │  Infrastructure │
└────────┬────────┘     └────────┬────────┘     └────────┬────────┘
         │                       │                       │
         │                       ▼                       │
         │             ┌─────────────────┐               │
         └────────────►│      Domain     │◄──────────────┘
                       └─────────────────┘
                                ▲
                                │
                        ┌────────────────┐
                        │   Persistence  │
                        └────────────────┘
```

> **Note**: Domain does not depend on any other layer, while all other layers depend on Domain. 
> Application doesn't depend on Infrastructure, Persistence, or Presentation.

## Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 9.0
- **Dependency Injection**: Autofac
- **Messaging**: MediatR
- **Object Mapping**: AutoMapper
- **Containerization**: Docker