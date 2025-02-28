# Full-Stack ERP System

This repository contains a full-stack Enterprise Resource Planning (ERP) system built with modern technologies. The system includes a modular backend API and a responsive frontend interface.

## Tech Stack

### Backend
- **.NET 9** with a Clean Architecture approach
- **Entity Framework Core** for data access
- **MediatR** for CQRS pattern implementation
- **SQL Server** for database (containerized)
- **Docker** for containerization

### Frontend
- **Next.js 14+** with App Router
- **React** with TypeScript
- **Tailwind CSS** for styling
- **Shadcn UI** component library

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18+)
- [Docker](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/downloads)

## Getting Started

### Using Docker (Recommended)

1. Clone the repository
   ```bash
   gh repo clone AxelOord/jwms-v2-prototype
   cd jwms-v2-prototype
   ```

2. Start the application stack
   ```bash
   docker-compose up -d
   ```

3. Access the application:
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5135
   - API Documentation: http://localhost:5135/swagger

### Manual Setup

#### Backend

1. Navigate to the backend directory
   ```bash
   cd apps/backend
   ```

2. Restore dependencies and build
   ```bash
   dotnet restore
   dotnet build
   ```

3. Run the API
   ```bash
   cd src/App
   dotnet run
   ```

#### Frontend

1. Navigate to the frontend directory
   ```bash
   cd apps/frontend
   ```

2. Install dependencies
   ```bash
   npm install
   ```

3. Start the development server
   ```bash
   npm run dev
   ```

## Project Structure

```
├── apps/
│   ├── backend/              # .NET backend application
│   │   ├── src/
│   │   │   ├── App/          # API entry point
│   │   │   ├── Common/       # Shared libraries
│   │   │   └── Modules/      # Business modules
│   │   └── test/             # Test projects
│   └── frontend/             # Next.js frontend application
│       ├── public/           # Static assets
│       └── src/              # Application source code
└── libs/                     # Shared libraries
```

## Development

### Backend Architecture

The backend follows Clean Architecture principles with:

- Domain layer: Contains entity models and business logic
- Application layer: Contains use cases and business rules
- Infrastructure layer: Contains data access, external services
- Presentation layer: Contains API controllers and DTOs

### Frontend Architecture

The frontend uses Next.js App Router with:

- React components for UI elements
- TypeScript for type-safe development
- Tailwind CSS for styling
- API client auto-generated from OpenAPI spec

## License

This project is licensed under the MIT License - see the LICENSE file for details.