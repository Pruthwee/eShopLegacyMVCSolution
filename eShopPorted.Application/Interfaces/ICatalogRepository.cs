using eShopPorted.Domain.Entities;

namespace eShopPorted.Application.Interfaces;

/// <summary>
/// Repository interface for catalog data access
/// </summary>
public interface ICatalogRepository
{
    Task<CatalogItem?> GetByIdAsync(int id);
    
    Task<IEnumerable<CatalogItem>> GetAllAsync();
    
    Task<(IEnumerable<CatalogItem> Items, long TotalCount)> GetPaginatedAsync(int pageSize, int pageIndex);
    
    Task<IEnumerable<CatalogType>> GetCatalogTypesAsync();
    
    Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync();
    
    Task AddAsync(CatalogItem item);
    
    Task UpdateAsync(CatalogItem item);
    
    Task DeleteAsync(CatalogItem item);
    
    Task<int> SaveChangesAsync();
}
