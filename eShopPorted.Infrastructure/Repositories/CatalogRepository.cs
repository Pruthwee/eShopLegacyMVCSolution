using eShopPorted.Domain.Entities;
using eShopPorted.Domain.Interfaces;
using eShopPorted.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eShopPorted.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for catalog operations
    /// </summary>
    public class CatalogRepository : ICatalogRepository
    {
        private readonly CatalogDbContext _context;

        public CatalogRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(int pageSize, int pageIndex)
        {
            return await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<long> GetTotalItemsCountAsync()
        {
            return await _context.CatalogItems.LongCountAsync();
        }

        public async Task<CatalogItem?> GetCatalogItemByIdAsync(int id)
        {
            return await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<IEnumerable<CatalogType>> GetCatalogTypesAsync()
        {
            return await _context.CatalogTypes.ToListAsync();
        }

        public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync()
        {
            return await _context.CatalogBrands.ToListAsync();
        }

        public async Task AddCatalogItemAsync(CatalogItem catalogItem)
        {
            await _context.CatalogItems.AddAsync(catalogItem);
        }

        public Task UpdateCatalogItemAsync(CatalogItem catalogItem)
        {
            _context.Entry(catalogItem).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteCatalogItemAsync(CatalogItem catalogItem)
        {
            _context.CatalogItems.Remove(catalogItem);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
