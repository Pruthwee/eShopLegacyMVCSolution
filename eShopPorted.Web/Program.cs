using eShopPorted.Application.Interfaces;
using eShopPorted.Application.Services;
using eShopPorted.Infrastructure.Data;
using eShopPorted.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eShopPorted.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline
        ConfigureMiddleware(app);

        // Seed database
        await SeedDatabaseAsync(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add MVC services
        services.AddControllersWithViews();

        // Configure database
        bool useMockData = configuration.GetValue<bool>("UseMockData");
        
        if (!useMockData)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register repository
            services.AddScoped<ICatalogRepository, CatalogRepository>();
            
            // Register service
            services.AddScoped<ICatalogService, CatalogService>();
        }
        else
        {
            // Register mock service
            services.AddSingleton<ICatalogService, CatalogServiceMock>();
        }

        // Add logging
        services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
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
    }

    private static async Task SeedDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        try
        {
            var context = services.GetService<CatalogDbContext>();
            if (context != null)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                var configuration = services.GetRequiredService<IConfiguration>();
                bool useCustomizationData = configuration.GetValue<bool>("UseCustomizationData");
                
                await CatalogDbContextSeed.SeedAsync(context, logger, useCustomizationData);
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
