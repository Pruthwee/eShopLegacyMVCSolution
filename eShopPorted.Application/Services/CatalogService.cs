using eShopPorted.Application.DTOs;
using eShopPorted.Application.Interfaces;
using eShopPorted.Domain.Entities;
using eShopPorted.Domain.Interfaces;

namespace eShopPorted.Application.Services
{
    /// <summary>
    /// Service implementation for catalog business logic
    /// </summary>
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _repository;

        public CatalogService(ICatalogRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItemsPaginatedAsync(int pageSize, int pageIndex)
        {
            var totalItems = await _repository.GetTotalItemsCountAsync();
            var itemsOnPage = await _repository.GetCatalogItemsAsync(pageSize, pageIndex);

            return new PaginatedItemsViewModel<CatalogItem>(
                pageIndex, pageSize, totalItems, itemsOnPage);
        }

        public async Task<CatalogItem?> FindCatalogItemAsync(int id)
        {
            return await _repository.GetCatalogItemByIdAsync(id);
        }

        public async Task<IEnumerable<CatalogType>> GetCatalogTypesAsync()
        {
            return await _repository.GetCatalogTypesAsync();
        }

        public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync()
        {
            return await _repository.GetCatalogBrandsAsync();
        }

        public async Task CreateCatalogItemAsync(CatalogItem catalogItem)
        {
            await _repository.AddCatalogItemAsync(catalogItem);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateCatalogItemAsync(CatalogItem catalogItem)
        {
            await _repository.UpdateCatalogItemAsync(catalogItem);
            await _repository.SaveChangesAsync();
        }

        public async Task RemoveCatalogItemAsync(CatalogItem catalogItem)
        {
            await _repository.DeleteCatalogItemAsync(catalogItem);
            await _repository.SaveChangesAsync();
        }
    }
}
