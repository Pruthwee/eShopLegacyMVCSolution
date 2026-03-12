using eShopPorted.Application.DTOs;
using eShopPorted.Domain.Entities;

namespace eShopPorted.Application.Interfaces
{
    /// <summary>
    /// Service interface for catalog business logic
    /// </summary>
    public interface ICatalogService
    {
        Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItemsPaginatedAsync(int pageSize, int pageIndex);
        Task<CatalogItem?> FindCatalogItemAsync(int id);
        Task<IEnumerable<CatalogType>> GetCatalogTypesAsync();
        Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync();
        Task CreateCatalogItemAsync(CatalogItem catalogItem);
        Task UpdateCatalogItemAsync(CatalogItem catalogItem);
        Task RemoveCatalogItemAsync(CatalogItem catalogItem);
    }
}
