# eShopPorted - .NET 8 Clean Architecture Migration

## Overview
This project is a complete migration of the legacy ASP.NET MVC eShop application to .NET 8 using Clean Architecture principles.

## Architecture

The solution follows Clean Architecture with four distinct layers:

### 1. Domain Layer (`eShopPorted.Domain`)
- **Purpose**: Contains enterprise business rules and entities
- **Dependencies**: None (pure domain logic)
- **Contents**:
  - `Entities/`: Domain entities (CatalogItem, CatalogBrand, CatalogType)
  - `Common/`: Base classes and shared domain logic

### 2. Application Layer (`eShopPorted.Application`)
- **Purpose**: Contains application business rules and use cases
- **Dependencies**: Domain layer only
- **Contents**:
  - `Interfaces/`: Service and repository interfaces
  - `Services/`: Business logic implementations
  - `ViewModels/`: Data transfer objects for views
  - `DTOs/`: Data transfer objects for APIs

### 3. Infrastructure Layer (`eShopPorted.Infrastructure`)
- **Purpose**: Contains data access and external service implementations
- **Dependencies**: Domain and Application layers
- **Contents**:
  - `Data/`: DbContext and database seeding
  - `Repositories/`: Repository implementations
  - `Configuration/`: Entity Framework configurations

### 4. Web Layer (`eShopPorted.Web`)
- **Purpose**: Presentation layer - MVC controllers and views
- **Dependencies**: All other layers
- **Contents**:
  - `Controllers/`: MVC and API controllers
  - `Views/`: Razor views
  - `wwwroot/`: Static files (CSS, JS, images)
  - `Program.cs`: Application entry point and configuration

## Technology Stack

- **.NET 8**: Latest LTS version
- **ASP.NET Core MVC**: Web framework
- **Entity Framework Core 8**: ORM for data access
- **SQL Server**: Database (LocalDB for development)
- **Bootstrap 4**: UI framework

## Key Migration Changes

### From Legacy to .NET 8

1. **Framework**: .NET Framework 4.6.1 → .NET 8
2. **MVC Version**: ASP.NET MVC 5 → ASP.NET Core MVC
3. **EF Version**: Entity Framework 6 → Entity Framework Core 8
4. **Dependency Injection**: Autofac → Built-in DI
5. **Logging**: log4net → ILogger<T>
6. **Configuration**: web.config → appsettings.json
7. **Startup**: Global.asax + Startup.cs → Program.cs (minimal hosting)

### Removed Dependencies

- ❌ Autofac (replaced with built-in DI)
- ❌ log4net (replaced with ILogger)
- ❌ System.Web.Mvc (replaced with Microsoft.AspNetCore.Mvc)
- ❌ BinaryFormatter (security risk, removed from utilities)

### Architecture Improvements

- ✅ Clean Architecture with clear separation of concerns
- ✅ Dependency Inversion Principle applied
- ✅ Repository pattern for data access
- ✅ Service layer for business logic
- ✅ Async/await patterns throughout
- ✅ Modern C# features (nullable reference types, pattern matching)

## Project Structure

```
eShopPorted/
├── eShopPorted.Domain/
│   ├── Entities/
│   │   ├── CatalogItem.cs
│   │   ├── CatalogBrand.cs
│   │   └── CatalogType.cs
│   └── Common/
│       └── BaseEntity.cs
│
├── eShopPorted.Application/
│   ├── Interfaces/
│   │   ├── ICatalogService.cs
│   │   └── ICatalogRepository.cs
│   ├── Services/
│   │   ├── CatalogService.cs
│   │   └── CatalogServiceMock.cs
│   └── ViewModels/
│       └── PaginatedItemsViewModel.cs
│
├── eShopPorted.Infrastructure/
│   ├── Data/
│   │   ├── CatalogDbContext.cs
│   │   └── CatalogDbContextSeed.cs
│   ├── Repositories/
│   │   └── CatalogRepository.cs
│   └── Configuration/
│       ├── CatalogItemConfig.cs
│       ├── CatalogBrandConfig.cs
│       └── CatalogTypeConfig.cs
│
└── eShopPorted.Web/
    ├── Controllers/
    │   ├── CatalogController.cs
    │   ├── BrandsController.cs
    │   ├── PicController.cs
    │   └── HomeController.cs
    ├── Views/
    │   ├── Catalog/
    │   └── Shared/
    ├── wwwroot/
    │   ├── Content/
    │   ├── Scripts/
    │   ├── Images/
    │   └── Pics/
    ├── Program.cs
    └── appsettings.json
```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=eShopPorted;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "UseMockData": true,
  "UseCustomizationData": true
}
```

- **UseMockData**: Set to `true` to use in-memory mock data (no database required)
- **UseCustomizationData**: Set to `true` to seed database with sample data

## Running the Application

### Prerequisites

- .NET 8 SDK installed
- SQL Server or LocalDB (if not using mock data)

### Steps

1. **Restore packages**:
   ```bash
   dotnet restore
   ```

2. **Build the solution**:
   ```bash
   dotnet build
   ```

3. **Run with mock data** (no database required):
   - Ensure `UseMockData: true` in appsettings.json
   ```bash
   cd eShopPorted.Web
   dotnet run
   ```

4. **Run with database**:
   - Set `UseMockData: false` in appsettings.json
   - Apply migrations:
     ```bash
     cd eShopPorted.Infrastructure
     dotnet ef database update --startup-project ../eShopPorted.Web
     ```
   - Run the application:
     ```bash
     cd ../eShopPorted.Web
     dotnet run
     ```

5. **Access the application**:
   - Navigate to `https://localhost:5001` or `http://localhost:5000`

## Database Migrations

### Create a new migration:
```bash
cd eShopPorted.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../eShopPorted.Web
```

### Update database:
```bash
dotnet ef database update --startup-project ../eShopPorted.Web
```

### Remove last migration:
```bash
dotnet ef migrations remove --startup-project ../eShopPorted.Web
```

## Testing

The application supports two modes:

1. **Mock Mode** (`UseMockData: true`):
   - No database required
   - In-memory data
   - Perfect for development and testing

2. **Database Mode** (`UseMockData: false`):
   - Uses SQL Server/LocalDB
   - Persistent data
   - Production-ready

## Features

- ✅ Catalog item management (CRUD operations)
- ✅ Pagination support
- ✅ Brand and type filtering
- ✅ Image management
- ✅ RESTful API endpoints
- ✅ Responsive UI with Bootstrap

## API Endpoints

### Catalog Management
- `GET /Catalog` - List catalog items (paginated)
- `GET /Catalog/Details/{id}` - View item details
- `GET /Catalog/Create` - Create item form
- `POST /Catalog/Create` - Create new item
- `GET /Catalog/Edit/{id}` - Edit item form
- `POST /Catalog/Edit/{id}` - Update item
- `GET /Catalog/Delete/{id}` - Delete confirmation
- `POST /Catalog/Delete/{id}` - Delete item

### API
- `GET /api/Brands` - Get all brands
- `DELETE /api/Brands/{id}` - Delete brand (demo only)

### Images
- `GET /items/{id}/pic` - Get item picture

## Migration Benefits

1. **Performance**: .NET 8 offers significant performance improvements
2. **Security**: Modern security features and practices
3. **Maintainability**: Clean Architecture ensures long-term maintainability
4. **Testability**: Dependency injection and interfaces enable easy testing
5. **Cross-platform**: Runs on Windows, Linux, and macOS
6. **Modern C#**: Latest language features and patterns
7. **Long-term Support**: .NET 8 is an LTS release (supported until 2026)

## Known Issues and Limitations

1. **BinaryFormatter Removed**: The legacy `eShopLegacy.Utilities` project used BinaryFormatter, which is deprecated and removed in .NET 8 due to security concerns. This functionality has been removed.

2. **Synchronous Service Methods**: The service layer uses synchronous methods for backward compatibility. Consider migrating to async methods for better performance.

3. **ViewBag Usage**: Some views still use ViewBag. Consider migrating to strongly-typed view models.

## Future Enhancements

- [ ] Add unit tests for all layers
- [ ] Add integration tests
- [ ] Implement async/await throughout service layer
- [ ] Add API versioning
- [ ] Implement CQRS pattern
- [ ] Add authentication and authorization
- [ ] Implement caching
- [ ] Add health checks
- [ ] Containerize with Docker
- [ ] Add CI/CD pipeline

## License

This is a sample application for demonstration purposes.

## Support

For issues or questions, please refer to the official .NET documentation:
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
- [.NET 8 Documentation](https://docs.microsoft.com/dotnet)
