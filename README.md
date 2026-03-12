# eShopPorted - .NET 8 Clean Architecture Migration

This project has been migrated from ASP.NET MVC 4.6.1 (ASP.NET Core 2.2) to .NET 8 with Clean Architecture.

## Architecture Overview

The solution follows Clean Architecture principles with clear separation of concerns:

```
eShopPorted/
├── eShopPorted.Domain/          # Core business entities and interfaces
│   ├── Entities/                # Domain entities (CatalogItem, CatalogBrand, CatalogType)
│   └── Interfaces/              # Repository interfaces
├── eShopPorted.Application/     # Business logic and use cases
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Interfaces/              # Service interfaces
│   └── Services/                # Service implementations
├── eShopPorted.Infrastructure/  # Data access and external concerns
│   ├── Data/                    # DbContext and configurations
│   └── Repositories/            # Repository implementations
└── eShopPorted.Web/             # Presentation layer (ASP.NET Core MVC)
    ├── Controllers/             # MVC Controllers
    ├── Views/                   # Razor views
    └── wwwroot/                 # Static files
```

## Key Changes from Legacy Application

### 1. Framework Migration
- **From**: .NET Framework 4.6.1 / ASP.NET Core 2.2
- **To**: .NET 8.0
- **Benefits**: Modern C# features, improved performance, long-term support

### 2. Architecture Pattern
- **From**: Single-project MVC structure
- **To**: Clean Architecture with 4 layers
- **Benefits**: Better testability, maintainability, and separation of concerns

### 3. Dependency Injection
- **From**: Autofac container
- **To**: Built-in .NET 8 DI container
- **Benefits**: Simplified configuration, native support

### 4. Entity Framework
- **From**: Entity Framework Core 2.2
- **To**: Entity Framework Core 8.0
- **Benefits**: Better performance, new features, bug fixes

### 5. Logging
- **From**: log4net
- **To**: Microsoft.Extensions.Logging (ILogger)
- **Benefits**: Native integration, structured logging

### 6. Configuration
- **From**: Startup.cs + Program.cs
- **To**: Minimal hosting model (Program.cs only)
- **Benefits**: Simplified configuration, less boilerplate

## Project Dependencies

### Domain Layer (eShopPorted.Domain)
- No external dependencies
- Pure business entities and interfaces

### Application Layer (eShopPorted.Application)
- Depends on: Domain
- Contains business logic and service interfaces

### Infrastructure Layer (eShopPorted.Infrastructure)
- Depends on: Domain
- Packages:
  - Microsoft.EntityFrameworkCore 8.0.0
  - Microsoft.EntityFrameworkCore.SqlServer 8.0.0
  - Microsoft.EntityFrameworkCore.Tools 8.0.0

### Web Layer (eShopPorted.Web)
- Depends on: Application, Infrastructure
- Packages:
  - Microsoft.EntityFrameworkCore.Design 8.0.0

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Running the Application

1. **Restore packages**:
   ```bash
   dotnet restore
   ```

2. **Build the solution**:
   ```bash
   dotnet build
   ```

3. **Run with mock data** (default):
   ```bash
   cd eShopPorted.Web
   dotnet run
   ```
   The application will use in-memory mock data.

4. **Run with database**:
   - Update `appsettings.json` to set `UseMockData: false`
   - Update connection string if needed
   - Run migrations:
     ```bash
     cd eShopPorted.Web
     dotnet ef database update
     ```
   - Run the application:
     ```bash
     dotnet run
     ```

### Database Migrations

The application uses Entity Framework Core migrations for database schema management.

**Create a new migration**:
```bash
cd eShopPorted.Web
dotnet ef migrations add MigrationName --project ../eShopPorted.Infrastructure
```

**Update database**:
```bash
dotnet ef database update
```

**Remove last migration**:
```bash
dotnet ef migrations remove --project ../eShopPorted.Infrastructure
```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=eShopPorted;..."
  },
  "UseMockData": true,  // Set to false to use database
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Testing

The application supports two modes:

1. **Mock Mode** (`UseMockData: true`):
   - Uses in-memory data
   - No database required
   - Perfect for development and testing

2. **Database Mode** (`UseMockData: false`):
   - Uses SQL Server database
   - Requires connection string configuration
   - Production-ready

## API Endpoints

### MVC Controllers
- `GET /Catalog` - List catalog items (paginated)
- `GET /Catalog/Details/{id}` - View item details
- `GET /Catalog/Create` - Create item form
- `POST /Catalog/Create` - Create item
- `GET /Catalog/Edit/{id}` - Edit item form
- `POST /Catalog/Edit/{id}` - Update item
- `GET /Catalog/Delete/{id}` - Delete confirmation
- `POST /Catalog/Delete/{id}` - Delete item

### API Controllers
- `GET /api/brands` - Get all brands
- `DELETE /api/brands/{id}` - Delete brand (demo only)
- `POST /api/files` - Upload file

### Static Files
- `GET /Pics/{id}.png` - Get catalog item image

## Migration Notes

### Breaking Changes Addressed

1. **Autofac Removal**: Replaced with built-in DI
2. **log4net Removal**: Replaced with ILogger
3. **Startup.cs**: Merged into Program.cs
4. **IWebHostBuilder**: Replaced with WebApplicationBuilder
5. **Synchronous methods**: Converted to async/await
6. **ActionResult**: Updated to IActionResult
7. **BadRequest()**: Now returns proper status codes

### Compatibility Maintained

- All routes preserved
- View structure unchanged
- UI/UX identical to original
- Database schema compatible

## Future Enhancements

- [ ] Add unit tests for all layers
- [ ] Add integration tests
- [ ] Implement CQRS pattern
- [ ] Add API versioning
- [ ] Implement caching
- [ ] Add health checks
- [ ] Add Swagger/OpenAPI documentation
- [ ] Implement authentication/authorization
- [ ] Add Docker support

## License

This is a demonstration project for ASP.NET MVC to .NET 8 migration.

## Support

For issues or questions, please refer to the migration documentation or contact the development team.
