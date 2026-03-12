using eShopPorted.Application.Interfaces;
using eShopPorted.Application.Services;
using eShopPorted.Domain.Interfaces;
using eShopPorted.Infrastructure.Data;
using eShopPorted.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure database
var useMockData = builder.Configuration.GetValue<bool>("UseMockData");

if (!useMockData)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<CatalogDbContext>(options =>
        options.UseSqlServer(connectionString));
    
    // Register repository
    builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
    builder.Services.AddScoped<ICatalogService, CatalogService>();
}
else
{
    // Use mock service for testing
    builder.Services.AddSingleton<ICatalogService, CatalogServiceMock>();
}

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Seed database if not using mock data
if (!useMockData)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<CatalogDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            
            // Apply migrations
            await context.Database.MigrateAsync();
            
            // Seed data
            await PreconfiguredData.SeedAsync(context, logger);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();
