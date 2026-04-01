using eShopPorted.Domain.Entities;
using eShopPorted.Application.ViewModels;

namespace eShopPorted.Application.Interfaces;

/// <summary>
/// Service interface for catalog operations
/// </summary>
public interface ICatalogService : IDisposable
{
    PaginatedItemsViewModel<CatalogItem> GetCatalogItemsPaginated(int pageSize, int pageIndex);
    
    CatalogItem? FindCatalogItem(int id);
    
    IEnumerable<CatalogType> GetCatalogTypes();
    
    IEnumerable<CatalogBrand> GetCatalogBrands();
    
    void CreateCatalogItem(CatalogItem catalogItem);
    
    void UpdateCatalogItem(CatalogItem catalogItem);
    
    void RemoveCatalogItem(CatalogItem catalogItem);
}
