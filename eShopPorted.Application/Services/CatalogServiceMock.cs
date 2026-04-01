using eShopPorted.Application.Interfaces;
using eShopPorted.Application.ViewModels;
using eShopPorted.Domain.Entities;

namespace eShopPorted.Application.Services;

/// <summary>
/// Mock catalog service for testing without database
/// </summary>
public class CatalogServiceMock : ICatalogService
{
    private List<CatalogItem> _catalogItems;
    private List<CatalogBrand> _catalogBrands;
    private List<CatalogType> _catalogTypes;

    public CatalogServiceMock()
    {
        InitializeMockData();
    }

    private void InitializeMockData()
    {
        _catalogBrands = new List<CatalogBrand>
        {
            new CatalogBrand { Id = 1, Brand = "Azure" },
            new CatalogBrand { Id = 2, Brand = ".NET" },
            new CatalogBrand { Id = 3, Brand = "Visual Studio" },
            new CatalogBrand { Id = 4, Brand = "SQL Server" },
            new CatalogBrand { Id = 5, Brand = "Other" }
        };

        _catalogTypes = new List<CatalogType>
        {
            new CatalogType { Id = 1, Type = "Mug" },
            new CatalogType { Id = 2, Type = "T-Shirt" },
            new CatalogType { Id = 3, Type = "Sheet" },
            new CatalogType { Id = 4, Type = "USB Memory Stick" }
        };

        _catalogItems = new List<CatalogItem>
        {
            new CatalogItem { Id = 1, CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Bot Black Hoodie", Name = ".NET Bot Black Hoodie", Price = 19.5M, PictureFileName = "1.png" },
            new CatalogItem { Id = 2, CatalogTypeId = 1, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price= 8.50M, PictureFileName = "2.png" },
            new CatalogItem { Id = 3, CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12, PictureFileName = "3.png" },
            new CatalogItem { Id = 4, CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation T-shirt", Name = ".NET Foundation T-shirt", Price = 12, PictureFileName = "4.png" },
            new CatalogItem { Id = 5, CatalogTypeId = 3, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5M, PictureFileName = "5.png" },
            new CatalogItem { Id = 6, CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Blue Hoodie", Name = ".NET Blue Hoodie", Price = 12, PictureFileName = "6.png" },
            new CatalogItem { Id = 7, CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12, PictureFileName = "7.png" },
            new CatalogItem { Id = 8, CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Kudu Purple Hoodie", Name = "Kudu Purple Hoodie", Price = 8.5M, PictureFileName = "8.png" },
            new CatalogItem { Id = 9, CatalogTypeId = 1, CatalogBrandId = 5, AvailableStock = 100, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12, PictureFileName = "9.png" },
            new CatalogItem { Id = 10, CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12, PictureFileName = "10.png" },
            new CatalogItem { Id = 11, CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5M, PictureFileName = "11.png" },
            new CatalogItem { Id = 12, CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White TShirt", Name = "Prism White TShirt", Price = 12, PictureFileName = "12.png" }
        };

        // Set navigation properties
        foreach (var item in _catalogItems)
        {
            item.CatalogBrand = _catalogBrands.FirstOrDefault(b => b.Id == item.CatalogBrandId);
            item.CatalogType = _catalogTypes.FirstOrDefault(t => t.Id == item.CatalogTypeId);
        }
    }

    public PaginatedItemsViewModel<CatalogItem> GetCatalogItemsPaginated(int pageSize, int pageIndex)
    {
        var totalItems = _catalogItems.Count;
        var itemsOnPage = _catalogItems
            .OrderBy(c => c.Id)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToList();

        return new PaginatedItemsViewModel<CatalogItem>(
            pageIndex, pageSize, totalItems, itemsOnPage);
    }

    public CatalogItem? FindCatalogItem(int id)
    {
        return _catalogItems.FirstOrDefault(ci => ci.Id == id);
    }

    public IEnumerable<CatalogType> GetCatalogTypes()
    {
        return _catalogTypes;
    }

    public IEnumerable<CatalogBrand> GetCatalogBrands()
    {
        return _catalogBrands;
    }

    public void CreateCatalogItem(CatalogItem catalogItem)
    {
        catalogItem.Id = _catalogItems.Max(i => i.Id) + 1;
        catalogItem.CatalogBrand = _catalogBrands.FirstOrDefault(b => b.Id == catalogItem.CatalogBrandId);
        catalogItem.CatalogType = _catalogTypes.FirstOrDefault(t => t.Id == catalogItem.CatalogTypeId);
        _catalogItems.Add(catalogItem);
    }

    public void UpdateCatalogItem(CatalogItem catalogItem)
    {
        var existingItem = _catalogItems.FirstOrDefault(i => i.Id == catalogItem.Id);
        if (existingItem != null)
        {
            existingItem.Name = catalogItem.Name;
            existingItem.Description = catalogItem.Description;
            existingItem.Price = catalogItem.Price;
            existingItem.PictureFileName = catalogItem.PictureFileName;
            existingItem.CatalogTypeId = catalogItem.CatalogTypeId;
            existingItem.CatalogBrandId = catalogItem.CatalogBrandId;
            existingItem.AvailableStock = catalogItem.AvailableStock;
            existingItem.RestockThreshold = catalogItem.RestockThreshold;
            existingItem.MaxStockThreshold = catalogItem.MaxStockThreshold;
            existingItem.OnReorder = catalogItem.OnReorder;
            existingItem.CatalogBrand = _catalogBrands.FirstOrDefault(b => b.Id == catalogItem.CatalogBrandId);
            existingItem.CatalogType = _catalogTypes.FirstOrDefault(t => t.Id == catalogItem.CatalogTypeId);
        }
    }

    public void RemoveCatalogItem(CatalogItem catalogItem)
    {
        _catalogItems.RemoveAll(i => i.Id == catalogItem.Id);
    }

    public void Dispose()
    {
        // Nothing to dispose in mock
    }
}
