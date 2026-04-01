using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using eShopPorted.Domain.Entities;
using eShopPorted.Application.Interfaces;

namespace eShopPorted.Web.Controllers;

/// <summary>
/// Controller for catalog management
/// </summary>
public class CatalogController : Controller
{
    private readonly ICatalogService _service;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ICatalogService service, ILogger<CatalogController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET /Catalog?pageSize=10&pageIndex=0
    public IActionResult Index(int pageSize = 10, int pageIndex = 0)
    {
        _logger.LogInformation("Loading catalog index page with pageSize={PageSize} and pageIndex={PageIndex}", pageSize, pageIndex);
        
        var paginatedItems = _service.GetCatalogItemsPaginated(pageSize, pageIndex);
        ChangeUriPlaceholder(paginatedItems.Data);
        
        return View(paginatedItems);
    }

    // GET: Catalog/Details/5
    public IActionResult Details(int? id)
    {
        _logger.LogInformation("Loading catalog details for id={Id}", id);
        
        if (id == null)
        {
            return BadRequest();
        }
        
        CatalogItem? catalogItem = _service.FindCatalogItem(id.Value);
        if (catalogItem == null)
        {
            return NotFound();
        }
        
        AddUriPlaceHolder(catalogItem);
        return View(catalogItem);
    }

    // GET: Catalog/Create
    public IActionResult Create()
    {
        _logger.LogInformation("Loading catalog create page");
        
        ViewBag.CatalogBrandId = new SelectList(_service.GetCatalogBrands(), "Id", "Brand");
        ViewBag.CatalogTypeId = new SelectList(_service.GetCatalogTypes(), "Id", "Type");
        
        return View(new CatalogItem());
    }

    // POST: Catalog/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,Name,Description,Price,PictureFileName,CatalogTypeId,CatalogBrandId,AvailableStock,RestockThreshold,MaxStockThreshold,OnReorder")] CatalogItem catalogItem)
    {
        _logger.LogInformation("Creating catalog item with name={Name}", catalogItem.Name);
        
        if (ModelState.IsValid)
        {
            _service.CreateCatalogItem(catalogItem);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.CatalogBrandId = new SelectList(_service.GetCatalogBrands(), "Id", "Brand", catalogItem.CatalogBrandId);
        ViewBag.CatalogTypeId = new SelectList(_service.GetCatalogTypes(), "Id", "Type", catalogItem.CatalogTypeId);
        
        return View(catalogItem);
    }

    // GET: Catalog/Edit/5
    public IActionResult Edit(int? id)
    {
        _logger.LogInformation("Loading catalog edit page for id={Id}", id);
        
        if (id == null)
        {
            return BadRequest();
        }
        
        CatalogItem? catalogItem = _service.FindCatalogItem(id.Value);
        if (catalogItem == null)
        {
            return NotFound();
        }
        
        AddUriPlaceHolder(catalogItem);
        ViewBag.CatalogBrandId = new SelectList(_service.GetCatalogBrands(), "Id", "Brand", catalogItem.CatalogBrandId);
        ViewBag.CatalogTypeId = new SelectList(_service.GetCatalogTypes(), "Id", "Type", catalogItem.CatalogTypeId);
        
        return View(catalogItem);
    }

    // POST: Catalog/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit([Bind("Id,Name,Description,Price,PictureFileName,CatalogTypeId,CatalogBrandId,AvailableStock,RestockThreshold,MaxStockThreshold,OnReorder")] CatalogItem catalogItem)
    {
        _logger.LogInformation("Updating catalog item with id={Id}", catalogItem.Id);
        
        if (ModelState.IsValid)
        {
            _service.UpdateCatalogItem(catalogItem);
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.CatalogBrandId = new SelectList(_service.GetCatalogBrands(), "Id", "Brand", catalogItem.CatalogBrandId);
        ViewBag.CatalogTypeId = new SelectList(_service.GetCatalogTypes(), "Id", "Type", catalogItem.CatalogTypeId);
        
        return View(catalogItem);
    }

    // GET: Catalog/Delete/5
    public IActionResult Delete(int? id)
    {
        _logger.LogInformation("Loading catalog delete page for id={Id}", id);
        
        if (id == null)
        {
            return BadRequest();
        }
        
        CatalogItem? catalogItem = _service.FindCatalogItem(id.Value);
        if (catalogItem == null)
        {
            return NotFound();
        }
        
        AddUriPlaceHolder(catalogItem);
        return View(catalogItem);
    }

    // POST: Catalog/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _logger.LogInformation("Deleting catalog item with id={Id}", id);
        
        CatalogItem? catalogItem = _service.FindCatalogItem(id);
        if (catalogItem != null)
        {
            _service.RemoveCatalogItem(catalogItem);
        }
        
        return RedirectToAction(nameof(Index));
    }

    private void ChangeUriPlaceholder(IEnumerable<CatalogItem> items)
    {
        foreach (var catalogItem in items)
        {
            AddUriPlaceHolder(catalogItem);
        }
    }

    private void AddUriPlaceHolder(CatalogItem item)
    {
        item.PictureUri = $"/Pics/{item.Id}.png";
    }
}
