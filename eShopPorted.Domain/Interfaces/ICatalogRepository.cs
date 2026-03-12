using eShopPorted.Domain.Entities;

namespace eShopPorted.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for catalog operations
    /// </summary>
    public interface ICatalogRepository
    {
        Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(int pageSize, int pageIndex);
        Task<long> GetTotalItemsCountAsync();
        Task<CatalogItem?> GetCatalogItemByIdAsync(int id);
        Task<IEnumerable<CatalogType>> GetCatalogTypesAsync();
        Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync();
        Task AddCatalogItemAsync(CatalogItem catalogItem);
        Task UpdateCatalogItemAsync(CatalogItem catalogItem);
        Task DeleteCatalogItemAsync(CatalogItem catalogItem);
        Task<int> SaveChangesAsync();
    }
}
