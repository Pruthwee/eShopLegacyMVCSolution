using Microsoft.EntityFrameworkCore;
using eShopPorted.Application.Interfaces;
using eShopPorted.Domain.Entities;
using eShopPorted.Infrastructure.Data;

namespace eShopPorted.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for catalog data access
/// </summary>
public class CatalogRepository : ICatalogRepository
{
    private readonly CatalogDbContext _context;

    public CatalogRepository(CatalogDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CatalogItem?> GetByIdAsync(int id)
    {
        return await _context.CatalogItems
            .Include(c => c.CatalogBrand)
            .Include(c => c.CatalogType)
            .FirstOrDefaultAsync(ci => ci.Id == id);
    }

    public async Task<IEnumerable<CatalogItem>> GetAllAsync()
    {
        return await _context.CatalogItems
            .Include(c => c.CatalogBrand)
            .Include(c => c.CatalogType)
            .ToListAsync();
    }

    public async Task<(IEnumerable<CatalogItem> Items, long TotalCount)> GetPaginatedAsync(int pageSize, int pageIndex)
    {
        var totalItems = await _context.CatalogItems.LongCountAsync();

        var itemsOnPage = await _context.CatalogItems
            .Include(c => c.CatalogBrand)
            .Include(c => c.CatalogType)
            .OrderBy(c => c.Id)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return (itemsOnPage, totalItems);
    }

    public async Task<IEnumerable<CatalogType>> GetCatalogTypesAsync()
    {
        return await _context.CatalogTypes.ToListAsync();
    }

    public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync()
    {
        return await _context.CatalogBrands.ToListAsync();
    }

    public async Task AddAsync(CatalogItem item)
    {
        await _context.CatalogItems.AddAsync(item);
    }

    public Task UpdateAsync(CatalogItem item)
    {
        _context.Entry(item).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(CatalogItem item)
    {
        _context.CatalogItems.Remove(item);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
