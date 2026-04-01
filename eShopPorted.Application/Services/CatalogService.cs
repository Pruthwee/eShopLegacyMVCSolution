using eShopPorted.Application.Interfaces;
using eShopPorted.Application.ViewModels;
using eShopPorted.Domain.Entities;

namespace eShopPorted.Application.Services;

/// <summary>
/// Catalog service implementation
/// </summary>
public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _repository;
    private bool _disposed = false;

    public CatalogService(ICatalogRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public PaginatedItemsViewModel<CatalogItem> GetCatalogItemsPaginated(int pageSize, int pageIndex)
    {
        var result = _repository.GetPaginatedAsync(pageSize, pageIndex).GetAwaiter().GetResult();
        
        return new PaginatedItemsViewModel<CatalogItem>(
            pageIndex, 
            pageSize, 
            result.TotalCount, 
            result.Items);
    }

    public CatalogItem? FindCatalogItem(int id)
    {
        return _repository.GetByIdAsync(id).GetAwaiter().GetResult();
    }

    public IEnumerable<CatalogType> GetCatalogTypes()
    {
        return _repository.GetCatalogTypesAsync().GetAwaiter().GetResult();
    }

    public IEnumerable<CatalogBrand> GetCatalogBrands()
    {
        return _repository.GetCatalogBrandsAsync().GetAwaiter().GetResult();
    }

    public void CreateCatalogItem(CatalogItem catalogItem)
    {
        _repository.AddAsync(catalogItem).GetAwaiter().GetResult();
        _repository.SaveChangesAsync().GetAwaiter().GetResult();
    }

    public void UpdateCatalogItem(CatalogItem catalogItem)
    {
        _repository.UpdateAsync(catalogItem).GetAwaiter().GetResult();
        _repository.SaveChangesAsync().GetAwaiter().GetResult();
    }

    public void RemoveCatalogItem(CatalogItem catalogItem)
    {
        _repository.DeleteAsync(catalogItem).GetAwaiter().GetResult();
        _repository.SaveChangesAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources if needed
            }
            _disposed = true;
        }
    }
}
