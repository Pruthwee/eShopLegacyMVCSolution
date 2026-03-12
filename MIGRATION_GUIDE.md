# ASP.NET MVC to .NET 8 Clean Architecture Migration Guide

## Executive Summary

This document provides a comprehensive guide for migrating the eShopPorted application from ASP.NET Core 2.2 targeting .NET Framework 4.6.1 to .NET 8 with Clean Architecture.

**Migration Complexity**: Medium to High
**Estimated Effort**: 40-60 hours
**Risk Level**: Medium

## Table of Contents

1. [Pre-Migration Assessment](#pre-migration-assessment)
2. [Architecture Transformation](#architecture-transformation)
3. [Step-by-Step Migration Process](#step-by-step-migration-process)
4. [Breaking Changes and Resolutions](#breaking-changes-and-resolutions)
5. [Testing Strategy](#testing-strategy)
6. [Deployment Considerations](#deployment-considerations)

## Pre-Migration Assessment

### Original Application Analysis

**Framework**: ASP.NET Core 2.2 on .NET Framework 4.6.1
**Architecture**: Single-project MVC
**Database**: Entity Framework Core 2.2 with SQL Server
**DI Container**: Autofac
**Logging**: log4net

### Key Dependencies Identified

```xml
<!-- Legacy Dependencies -->
<PackageReference Include="Autofac" Version="4.9.1" />
<PackageReference Include="Autofac.Extensions.DependencyInjection" Version="4.4.0" />
<PackageReference Include="Autofac.Mvc5" Version="4.0.2" />
<PackageReference Include="log4net" Version="2.0.10" />
<PackageReference Include="Microsoft.AspNetCore" Version="2.2.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="2.2.6" />
```

### Migration Blockers Identified

1. ✅ **RESOLVED**: Autofac dependency - Replaced with built-in DI
2. ✅ **RESOLVED**: log4net - Replaced with ILogger
3. ✅ **RESOLVED**: .NET Framework 4.6.1 target - Upgraded to .NET 8
4. ✅ **RESOLVED**: EF Core 2.2 - Upgraded to EF Core 8.0
5. ✅ **RESOLVED**: Startup.cs pattern - Migrated to minimal hosting
6. ✅ **RESOLVED**: Synchronous methods - Converted to async/await

## Architecture Transformation

### From: Single-Project MVC

```
eShopPorted/
├── Controllers/
├── Models/
├── Views/
├── Services/
├── Startup.cs
└── Program.cs
```

### To: Clean Architecture (4 Layers)

```
eShopPorted/
├── eShopPorted.Domain/          # Enterprise Business Rules
│   ├── Entities/
│   └── Interfaces/
├── eShopPorted.Application/     # Application Business Rules
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
├── eShopPorted.Infrastructure/  # Frameworks & Drivers
│   ├── Data/
│   └── Repositories/
└── eShopPorted.Web/             # Interface Adapters
    ├── Controllers/
    ├── Views/
    └── wwwroot/
```

### Dependency Flow

```
Web → Application → Domain
Web → Infrastructure → Domain
Infrastructure → Domain
```

**Key Principle**: Dependencies point inward. Domain has no dependencies.

## Step-by-Step Migration Process

### Phase 1: Create Domain Layer (2-4 hours)

1. **Create Domain Project**
   ```bash
   dotnet new classlib -n eShopPorted.Domain -f net8.0
   ```

2. **Move Entities**
   - CatalogItem.cs
   - CatalogBrand.cs
   - CatalogType.cs

3. **Define Repository Interfaces**
   - ICatalogRepository.cs

4. **Remove EF Dependencies**
   - Domain should have NO external dependencies
   - Pure POCOs only

### Phase 2: Create Application Layer (4-6 hours)

1. **Create Application Project**
   ```bash
   dotnet new classlib -n eShopPorted.Application -f net8.0
   dotnet add reference ../eShopPorted.Domain/eShopPorted.Domain.csproj
   ```

2. **Move Business Logic**
   - Service interfaces (ICatalogService)
   - Service implementations (CatalogService, CatalogServiceMock)
   - DTOs (PaginatedItemsViewModel)

3. **Convert to Async**
   - All service methods should return Task<T>
   - Use async/await throughout

### Phase 3: Create Infrastructure Layer (6-8 hours)

1. **Create Infrastructure Project**
   ```bash
   dotnet new classlib -n eShopPorted.Infrastructure -f net8.0
   dotnet add reference ../eShopPorted.Domain/eShopPorted.Domain.csproj
   ```

2. **Add EF Core Packages**
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
   dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
   ```

3. **Move Data Access**
   - DbContext (CatalogDbContext)
   - Entity configurations
   - Repository implementations
   - Data seeding logic

4. **Update DbContext**
   ```csharp
   public class CatalogDbContext : DbContext
   {
       public CatalogDbContext(DbContextOptions<CatalogDbContext> options) 
           : base(options) { }
       
       // DbSets...
   }
   ```

### Phase 4: Create Web Layer (8-12 hours)

1. **Create Web Project**
   ```bash
   dotnet new mvc -n eShopPorted.Web -f net8.0
   dotnet add reference ../eShopPorted.Application/eShopPorted.Application.csproj
   dotnet add reference ../eShopPorted.Infrastructure/eShopPorted.Infrastructure.csproj
   ```

2. **Migrate Controllers**
   - Update to async/await
   - Replace ActionResult with IActionResult
   - Update logging to ILogger<T>
   - Remove Autofac dependencies

3. **Update Program.cs**
   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   
   // Add services
   builder.Services.AddControllersWithViews();
   builder.Services.AddDbContext<CatalogDbContext>(options =>
       options.UseSqlServer(connectionString));
   builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
   builder.Services.AddScoped<ICatalogService, CatalogService>();
   
   var app = builder.Build();
   
   // Configure pipeline
   app.UseStaticFiles();
   app.UseRouting();
   app.MapControllerRoute(...);
   
   app.Run();
   ```

4. **Migrate Views**
   - Copy all .cshtml files
   - Create _ViewImports.cshtml
   - Update namespaces
   - Remove Web.config from Views folder

5. **Copy Static Files**
   - Copy wwwroot folder
   - Verify paths in views

### Phase 5: Configuration & DI (4-6 hours)

1. **Update appsettings.json**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "..."
     },
     "UseMockData": true,
     "Logging": {
       "LogLevel": {
         "Default": "Information"
       }
     }
   }
   ```

2. **Configure Services**
   - Register DbContext
   - Register repositories
   - Register services
   - Configure logging

3. **Remove Autofac**
   - Delete ApplicationModule.cs
   - Use built-in DI container
   - Update service registrations

### Phase 6: Testing & Validation (8-12 hours)

1. **Build Solution**
   ```bash
   dotnet build
   ```

2. **Run Migrations**
   ```bash
   dotnet ef database update --project eShopPorted.Web
   ```

3. **Test Mock Mode**
   - Set UseMockData = true
   - Run application
   - Test all CRUD operations

4. **Test Database Mode**
   - Set UseMockData = false
   - Verify database connection
   - Test all CRUD operations

5. **Verify API Endpoints**
   - Test all MVC routes
   - Test API controllers
   - Verify static file serving

## Breaking Changes and Resolutions

### 1. Autofac → Built-in DI

**Before**:
```csharp
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    var builder = new ContainerBuilder();
    builder.Populate(services);
    builder.RegisterModule(new ApplicationModule(useMockData));
    return new AutofacServiceProvider(builder.Build());
}
```

**After**:
```csharp
builder.Services.AddScoped<ICatalogService, CatalogService>();
// or
builder.Services.AddSingleton<ICatalogService, CatalogServiceMock>();
```

### 2. log4net → ILogger

**Before**:
```csharp
private static readonly ILog _log = LogManager.GetLogger(
    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
_log.Info($"Now loading... /Catalog/Index");
```

**After**:
```csharp
private readonly ILogger<CatalogController> _logger;

public CatalogController(ILogger<CatalogController> logger)
{
    _logger = logger;
}

_logger.LogInformation("Loading catalog index page");
```

### 3. Startup.cs → Program.cs

**Before**:
```csharp
// Program.cs
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

// Startup.cs
public void ConfigureServices(IServiceCollection services) { }
public void Configure(IApplicationBuilder app) { }
```

**After**:
```csharp
// Program.cs only
var builder = WebApplication.CreateBuilder(args);
// Add services
var app = builder.Build();
// Configure pipeline
app.Run();
```

### 4. Synchronous → Async

**Before**:
```csharp
public ActionResult Index(int pageSize = 10, int pageIndex = 0)
{
    var items = service.GetCatalogItemsPaginated(pageSize, pageIndex);
    return View(items);
}
```

**After**:
```csharp
public async Task<IActionResult> Index(int pageSize = 10, int pageIndex = 0)
{
    var items = await _service.GetCatalogItemsPaginatedAsync(pageSize, pageIndex);
    return View(items);
}
```

### 5. EF Core 2.2 → 8.0

**Before**:
```csharp
services.AddDbContext<CatalogDBContext>(options =>
    options.UseSqlServer(connectionString));
```

**After**:
```csharp
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(connectionString));
```

**Key Changes**:
- Updated package versions
- Improved performance
- New features (e.g., compiled models)
- Better async support

### 6. ActionResult → IActionResult

**Before**:
```csharp
public ActionResult Details(int? id)
{
    return View(catalogItem);
}
```

**After**:
```csharp
public async Task<IActionResult> Details(int? id)
{
    return View(catalogItem);
}
```

### 7. Dispose Pattern

**Before**:
```csharp
protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        service.Dispose();
    }
    base.Dispose(disposing);
}
```

**After**:
```csharp
// Not needed - DI container handles disposal
// Services are scoped and disposed automatically
```

## Testing Strategy

### Unit Testing

1. **Domain Layer Tests**
   - Test entity validation
   - Test business rules

2. **Application Layer Tests**
   - Test service logic
   - Mock repository dependencies
   - Test DTOs

3. **Infrastructure Layer Tests**
   - Test repository implementations
   - Use in-memory database
   - Test data seeding

4. **Web Layer Tests**
   - Test controller actions
   - Mock service dependencies
   - Test view models

### Integration Testing

1. **API Tests**
   - Test all endpoints
   - Verify status codes
   - Validate responses

2. **Database Tests**
   - Test migrations
   - Test data seeding
   - Test CRUD operations

3. **End-to-End Tests**
   - Test complete workflows
   - Test UI interactions
   - Test error handling

## Deployment Considerations

### Prerequisites

- .NET 8 Runtime
- SQL Server (2016 or later)
- IIS 10+ or Kestrel

### Configuration

1. **Update Connection Strings**
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=prod-server;Database=eShop;..."
   }
   ```

2. **Set Environment**
   ```bash
   export ASPNETCORE_ENVIRONMENT=Production
   ```

3. **Disable Mock Data**
   ```json
   "UseMockData": false
   ```

### Deployment Steps

1. **Publish Application**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Run Migrations**
   ```bash
   dotnet ef database update --project eShopPorted.Web
   ```

3. **Deploy to Server**
   - Copy publish folder
   - Configure IIS or systemd
   - Set environment variables

4. **Verify Deployment**
   - Test health endpoint
   - Verify database connection
   - Test key features

### Rollback Plan

1. **Database Rollback**
   ```bash
   dotnet ef database update PreviousMigration
   ```

2. **Application Rollback**
   - Restore previous version
   - Restart application
   - Verify functionality

## Performance Considerations

### Improvements in .NET 8

- **Faster startup time**: ~30% improvement
- **Lower memory usage**: ~20% reduction
- **Better throughput**: ~40% improvement
- **Async improvements**: Better async/await performance

### Optimization Tips

1. **Use async/await consistently**
2. **Enable response compression**
3. **Use output caching**
4. **Optimize database queries**
5. **Use compiled models (EF Core 8)**

## Security Considerations

### Updates Required

1. **Update NuGet packages** to latest versions
2. **Review authentication** implementation
3. **Enable HTTPS** redirection
4. **Configure CORS** if needed
5. **Implement rate limiting**
6. **Add security headers**

### Best Practices

- Use parameterized queries (EF Core handles this)
- Validate all inputs
- Use anti-forgery tokens
- Implement proper error handling
- Log security events

## Monitoring and Logging

### Logging Configuration

```csharp
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventLog(); // Windows only
```

### Recommended Logging Levels

- **Development**: Debug
- **Staging**: Information
- **Production**: Warning

### Monitoring Tools

- Application Insights
- Seq
- Serilog
- ELK Stack

## Conclusion

This migration successfully transforms the eShopPorted application from a legacy ASP.NET Core 2.2 application to a modern .NET 8 application with Clean Architecture. The new architecture provides:

- ✅ Better separation of concerns
- ✅ Improved testability
- ✅ Enhanced maintainability
- ✅ Modern async patterns
- ✅ Latest framework features
- ✅ Long-term support (LTS)

The application is now ready for production deployment and future enhancements.
