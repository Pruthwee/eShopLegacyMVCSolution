# Migration Guide: ASP.NET MVC to .NET 8 Clean Architecture

## Executive Summary

This document provides a comprehensive guide for migrating the eShopPorted application from ASP.NET MVC 4.6.1 to .NET 8 with Clean Architecture.

## Migration Overview

### Source Application
- **Framework**: .NET Framework 4.6.1
- **MVC Version**: ASP.NET MVC (hybrid with ASP.NET Core 2.2)
- **Architecture**: Monolithic MVC
- **Data Access**: Entity Framework Core 2.2
- **DI Container**: Autofac
- **Logging**: log4net

### Target Application
- **Framework**: .NET 8
- **MVC Version**: ASP.NET Core MVC
- **Architecture**: Clean Architecture (4 layers)
- **Data Access**: Entity Framework Core 8
- **DI Container**: Built-in .NET DI
- **Logging**: ILogger<T>

## Architecture Transformation

### Clean Architecture Layers

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│        (eShopPorted.Web)                │
│  Controllers, Views, API Endpoints      │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│       Application Layer                 │
│     (eShopPorted.Application)           │
│  Services, Interfaces, ViewModels       │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│      Infrastructure Layer               │
│    (eShopPorted.Infrastructure)         │
│  Repositories, DbContext, EF Config     │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│         Domain Layer                    │
│       (eShopPorted.Domain)              │
│    Entities, Business Rules             │
└─────────────────────────────────────────┘
```

### Dependency Flow

- **Domain**: No dependencies (pure business logic)
- **Application**: Depends on Domain
- **Infrastructure**: Depends on Domain and Application
- **Web**: Depends on all layers

## Step-by-Step Migration Process

### Phase 1: Project Structure Setup

1. **Create Solution Structure**
   ```bash
   mkdir eShopPorted.Domain
   mkdir eShopPorted.Application
   mkdir eShopPorted.Infrastructure
   mkdir eShopPorted.Web
   ```

2. **Create Project Files**
   - Domain: Class library targeting net8.0
   - Application: Class library targeting net8.0
   - Infrastructure: Class library targeting net8.0
   - Web: ASP.NET Core Web App targeting net8.0

3. **Set Up Project References**
   - Application → Domain
   - Infrastructure → Domain, Application
   - Web → Domain, Application, Infrastructure

### Phase 2: Domain Layer Migration

1. **Move Entity Classes**
   - Source: `eShopPorted/Models/CatalogItem.cs`
   - Target: `eShopPorted.Domain/Entities/CatalogItem.cs`
   - Changes:
     - Update namespace
     - Enable nullable reference types
     - Remove EF-specific attributes (move to configuration)

2. **Create Base Classes**
   - Add `BaseEntity.cs` for common properties
   - Add domain interfaces if needed

### Phase 3: Application Layer Migration

1. **Define Interfaces**
   - Create `ICatalogService.cs`
   - Create `ICatalogRepository.cs`
   - Define contracts for all operations

2. **Implement Services**
   - Move business logic from controllers to services
   - Implement `CatalogService.cs`
   - Implement `CatalogServiceMock.cs` for testing

3. **Create ViewModels**
   - Move `PaginatedItemsViewModel.cs`
   - Update to use generic types
   - Remove framework-specific dependencies

### Phase 4: Infrastructure Layer Migration

1. **Migrate DbContext**
   - Source: `eShopPorted/Models/CatalogDBContext.cs`
   - Target: `eShopPorted.Infrastructure/Data/CatalogDbContext.cs`
   - Changes:
     - Update to EF Core 8
     - Use fluent API for configuration
     - Remove obsolete methods

2. **Create Entity Configurations**
   - Extract EF configuration to separate classes
   - Implement `IEntityTypeConfiguration<T>`
   - Configure relationships, constraints, and indexes

3. **Implement Repositories**
   - Create `CatalogRepository.cs`
   - Implement async methods
   - Use EF Core 8 features

4. **Create Data Seeding**
   - Implement `CatalogDbContextSeed.cs`
   - Move preconfigured data
   - Add seeding logic to Program.cs

### Phase 5: Web Layer Migration

1. **Update Program.cs**
   - Remove old `Startup.cs` pattern
   - Use minimal hosting model
   - Configure services inline
   - Set up middleware pipeline

2. **Migrate Controllers**
   - Update namespaces
   - Replace Autofac DI with constructor injection
   - Replace log4net with ILogger<T>
   - Remove System.Web dependencies
   - Update action result types

3. **Update Views**
   - Create `_ViewImports.cshtml`
   - Update model references
   - Fix namespace issues
   - Update helper methods

4. **Migrate Static Files**
   - Copy wwwroot folder
   - Update paths in views
   - Ensure proper structure

### Phase 6: Configuration Migration

1. **Replace web.config**
   - Create `appsettings.json`
   - Move connection strings
   - Move app settings
   - Create environment-specific configs

2. **Update Dependency Injection**
   - Remove Autofac
   - Use built-in DI
   - Register services in Program.cs
   - Configure lifetimes (Scoped, Singleton, Transient)

### Phase 7: Package Migration

1. **Remove Legacy Packages**
   ```xml
   <!-- Remove -->
   <PackageReference Include="Autofac" />
   <PackageReference Include="Autofac.Extensions.DependencyInjection" Version="4.4.0" />
   <PackageReference Include="Autofac.Mvc5" />
   <PackageReference Include="log4net" />
   <PackageReference Include="Microsoft.AspNetCore" Version="2.2.0" />
   <PackageReference Include="Microsoft.EntityFrameworkCore" Version="2.2.6" />
   ```

2. **Add .NET 8 Packages**
   ```xml
   <!-- Add -->
   <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
   <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
   <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
   ```

### Phase 8: Testing and Validation

1. **Build Verification**
   ```bash
   dotnet build
   ```

2. **Run with Mock Data**
   ```bash
   cd eShopPorted.Web
   dotnet run
   ```

3. **Test All Features**
   - List catalog items
   - Create new item
   - Edit item
   - Delete item
   - View details
   - Pagination

4. **Database Testing**
   - Set `UseMockData: false`
   - Run migrations
   - Test with real database

## Breaking Changes and Resolutions

### 1. Autofac → Built-in DI

**Before:**
```csharp
var builder = new ContainerBuilder();
builder.RegisterType<CatalogService>().As<ICatalogService>();
```

**After:**
```csharp
services.AddScoped<ICatalogService, CatalogService>();
```

### 2. log4net → ILogger

**Before:**
```csharp
private static readonly ILog _log = LogManager.GetLogger(typeof(CatalogController));
_log.Info("Message");
```

**After:**
```csharp
private readonly ILogger<CatalogController> _logger;
_logger.LogInformation("Message");
```

### 3. System.Web.Mvc → Microsoft.AspNetCore.Mvc

**Before:**
```csharp
using System.Web.Mvc;
public ActionResult Index() { }
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
```

**After:**
```csharp
using Microsoft.AspNetCore.Mvc;
public IActionResult Index() { }
return BadRequest();
```

### 4. Global.asax → Program.cs

**Before:**
```csharp
// Global.asax.cs
protected void Application_Start()
{
    RouteConfig.RegisterRoutes(RouteTable.Routes);
}
```

**After:**
```csharp
// Program.cs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");
```

### 5. web.config → appsettings.json

**Before:**
```xml
<connectionStrings>
  <add name="DefaultConnection" connectionString="..." />
</connectionStrings>
```

**After:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  }
}
```

### 6. EF 2.2 → EF Core 8

**Before:**
```csharp
services.AddDbContext<CatalogDBContext>(options =>
    options.UseSqlServer(connectionString));
```

**After:**
```csharp
services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(connectionString));
```

### 7. BinaryFormatter Removal

**Before:**
```csharp
var binaryFormatter = new BinaryFormatter();
binaryFormatter.Serialize(stream, input);
```

**After:**
```csharp
// Use JSON serialization instead
var json = JsonSerializer.Serialize(input);
```

## Common Issues and Solutions

### Issue 1: Namespace Conflicts

**Problem**: Views can't find model types

**Solution**: Add `_ViewImports.cshtml`:
```csharp
@using eShopPorted.Web
@using eShopPorted.Domain.Entities
@using eShopPorted.Application.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

### Issue 2: Async/Await Patterns

**Problem**: Mixing sync and async code

**Solution**: Use async throughout:
```csharp
public async Task<IActionResult> Index()
{
    var items = await _service.GetCatalogItemsAsync();
    return View(items);
}
```

### Issue 3: Dependency Injection Lifetime

**Problem**: DbContext disposed too early

**Solution**: Use correct lifetime:
```csharp
services.AddScoped<CatalogDbContext>(); // Scoped per request
services.AddScoped<ICatalogRepository, CatalogRepository>();
```

### Issue 4: Static Files Not Serving

**Problem**: CSS/JS files return 404

**Solution**: Ensure middleware order:
```csharp
app.UseStaticFiles(); // Before UseRouting
app.UseRouting();
app.MapControllers();
```

## Performance Improvements

### 1. Async/Await
- All database operations are async
- Improves scalability
- Better resource utilization

### 2. EF Core 8 Optimizations
- Compiled queries
- Split queries for includes
- Better change tracking

### 3. Minimal Hosting Model
- Faster startup time
- Reduced memory footprint
- Simplified configuration

## Security Enhancements

### 1. Removed BinaryFormatter
- Security vulnerability in .NET Framework
- Replaced with JSON serialization

### 2. Modern Authentication
- Ready for ASP.NET Core Identity
- Support for JWT tokens
- OAuth 2.0 / OpenID Connect ready

### 3. HTTPS by Default
- Enforced in production
- HSTS headers
- Secure cookies

## Testing Strategy

### Unit Tests
```csharp
[Fact]
public async Task GetCatalogItems_ReturnsItems()
{
    // Arrange
    var mockRepo = new Mock<ICatalogRepository>();
    var service = new CatalogService(mockRepo.Object);
    
    // Act
    var result = await service.GetCatalogItemsAsync();
    
    // Assert
    Assert.NotNull(result);
}
```

### Integration Tests
```csharp
[Fact]
public async Task CatalogController_Index_ReturnsView()
{
    // Arrange
    var client = _factory.CreateClient();
    
    // Act
    var response = await client.GetAsync("/Catalog");
    
    // Assert
    response.EnsureSuccessStatusCode();
}
```

## Deployment Considerations

### 1. Self-Contained Deployment
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

### 2. Framework-Dependent Deployment
```bash
dotnet publish -c Release
```

### 3. Docker Containerization
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY publish/ .
ENTRYPOINT ["dotnet", "eShopPorted.Web.dll"]
```

## Rollback Plan

1. **Keep Legacy Application**
   - Maintain original codebase
   - Run side-by-side during transition

2. **Database Compatibility**
   - EF Core 8 can work with existing schema
   - Test migrations thoroughly

3. **Feature Flags**
   - Gradual rollout
   - Easy rollback per feature

## Success Criteria

- ✅ Application builds without errors
- ✅ All features work as expected
- ✅ Performance meets or exceeds legacy app
- ✅ No security regressions
- ✅ Clean Architecture principles followed
- ✅ Code is maintainable and testable

## Timeline Estimate

- **Phase 1-2**: 1-2 days (Project setup and Domain)
- **Phase 3-4**: 2-3 days (Application and Infrastructure)
- **Phase 5-6**: 2-3 days (Web and Configuration)
- **Phase 7-8**: 1-2 days (Packages and Testing)

**Total**: 6-10 days for a small application

## Conclusion

This migration transforms a legacy ASP.NET MVC application into a modern, maintainable .NET 8 application following Clean Architecture principles. The new architecture provides better separation of concerns, improved testability, and long-term maintainability.

## Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET 8 Migration Guide](https://docs.microsoft.com/dotnet/core/migration)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
