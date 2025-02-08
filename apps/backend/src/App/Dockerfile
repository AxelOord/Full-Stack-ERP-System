# Customizing your debug container for faster debugging
# See https://aka.ms/customizecontainer for more details

# Base image for running the application in debug mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5135 7013

# Image for building the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /root

# Executable
COPY ["src/App/App.csproj", "src/App/"]

# Common
COPY ["src/Common/Infrastructure/Infrastructure.csproj", "src/Common/Infrastructure/"]
COPY ["src/Common/Application/Application.csproj", "src/Common/Application/"]
COPY ["src/Common/Domain/Domain.csproj", "src/Common/Domain/"]
COPY ["src/Common/Shared/Shared.csproj", "src/Common/Shared/"]
COPY ["src/Common/Persistence/Persistence.csproj", "src/Common/Persistence/"]
COPY ["src/Common/Presentation/Presentation.csproj", "src/Common/Presentation/"]

# Modules - Warehouse
COPY ["src/Modules/Warehouse/Infrastructure/Warehouse.Infrastructure.csproj", "src/Modules/Warehouse/Infrastructure/"]
COPY ["src/Modules/Warehouse/Application/Warehouse.Application.csproj", "src/Modules/Warehouse/Application/"]
COPY ["src/Modules/Warehouse/Domain/Warehouse.Domain.csproj", "src/Modules/Warehouse/Domain/"]
COPY ["src/Modules/Warehouse/Persistence/Warehouse.Persistence.csproj", "src/Modules/Warehouse/Persistence/"]
COPY ["src/Modules/Warehouse/Presentation/Warehouse.Presentation.csproj", "src/Modules/Warehouse/Presentation/"]

# Restore dependencies
RUN dotnet restore "src/App/App.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/root/src/App"
RUN dotnet build "App.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false && ls -R /app/publish

# Final image for production or regular mode in VS (non-Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish /app

# Run the application
ENTRYPOINT ["dotnet", "App.dll"]
