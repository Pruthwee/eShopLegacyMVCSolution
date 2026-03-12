using eShopPorted.Application.Interfaces;
using eShopPorted.Domain.Entities;
using eShopPorted.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShopPorted.Web.Controllers
{
    /// <summary>
    /// Controller for catalog management operations
    /// </summary>
    public class CatalogController : Controller
    {
        private readonly ICatalogService _service;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ICatalogService service, ILogger<CatalogController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET /Catalog?pageSize=10&pageIndex=0
        public async Task<IActionResult> Index(int pageSize = 10, int pageIndex = 0)
        {
            _logger.LogInformation("Loading catalog index page with pageSize={PageSize} and pageIndex={PageIndex}", pageSize, pageIndex);
            
            var paginatedItems = await _service.GetCatalogItemsPaginatedAsync(pageSize, pageIndex);
            ChangeUriPlaceholder(paginatedItems.Data);
            
            // Convert to Web ViewModel
            var viewModel = new PaginatedItemsViewModel<CatalogItem>
            {
                PageIndex = paginatedItems.PageIndex,
                PageSize = paginatedItems.PageSize,
                Count = paginatedItems.Count,
                Data = paginatedItems.Data
            };
            
            return View(viewModel);
        }

        // GET: Catalog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null id");
                return BadRequest();
            }

            _logger.LogInformation("Loading catalog item details for id={Id}", id);
            
            var catalogItem = await _service.FindCatalogItemAsync(id.Value);
            if (catalogItem == null)
            {
                _logger.LogWarning("Catalog item with id={Id} not found", id);
                return NotFound();
            }
            
            AddUriPlaceHolder(catalogItem);
            return View(catalogItem);
        }

        // GET: Catalog/Create
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Loading catalog create page");
            
            ViewBag.CatalogBrandId = new SelectList(await _service.GetCatalogBrandsAsync(), "Id", "Brand");
            ViewBag.CatalogTypeId = new SelectList(await _service.GetCatalogTypesAsync(), "Id", "Type");
            
            return View(new CatalogItem());
        }

        // POST: Catalog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,PictureFileName,CatalogTypeId,CatalogBrandId,AvailableStock,RestockThreshold,MaxStockThreshold,OnReorder")] CatalogItem catalogItem)
        {
            _logger.LogInformation("Creating catalog item with name={Name}", catalogItem.Name);
            
            if (ModelState.IsValid)
            {
                await _service.CreateCatalogItemAsync(catalogItem);
                _logger.LogInformation("Successfully created catalog item with id={Id}", catalogItem.Id);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CatalogBrandId = new SelectList(await _service.GetCatalogBrandsAsync(), "Id", "Brand", catalogItem.CatalogBrandId);
            ViewBag.CatalogTypeId = new SelectList(await _service.GetCatalogTypesAsync(), "Id", "Type", catalogItem.CatalogTypeId);
            
            return View(catalogItem);
        }

        // GET: Catalog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null id");
                return BadRequest();
            }

            _logger.LogInformation("Loading catalog edit page for id={Id}", id);
            
            var catalogItem = await _service.FindCatalogItemAsync(id.Value);
            if (catalogItem == null)
            {
                _logger.LogWarning("Catalog item with id={Id} not found", id);
                return NotFound();
            }
            
            AddUriPlaceHolder(catalogItem);
            ViewBag.CatalogBrandId = new SelectList(await _service.GetCatalogBrandsAsync(), "Id", "Brand", catalogItem.CatalogBrandId);
            ViewBag.CatalogTypeId = new SelectList(await _service.GetCatalogTypesAsync(), "Id", "Type", catalogItem.CatalogTypeId);
            
            return View(catalogItem);
        }

        // POST: Catalog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Description,Price,PictureFileName,CatalogTypeId,CatalogBrandId,AvailableStock,RestockThreshold,MaxStockThreshold,OnReorder")] CatalogItem catalogItem)
        {
            _logger.LogInformation("Updating catalog item with id={Id}", catalogItem.Id);
            
            if (ModelState.IsValid)
            {
                await _service.UpdateCatalogItemAsync(catalogItem);
                _logger.LogInformation("Successfully updated catalog item with id={Id}", catalogItem.Id);
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.CatalogBrandId = new SelectList(await _service.GetCatalogBrandsAsync(), "Id", "Brand", catalogItem.CatalogBrandId);
            ViewBag.CatalogTypeId = new SelectList(await _service.GetCatalogTypesAsync(), "Id", "Type", catalogItem.CatalogTypeId);
            
            return View(catalogItem);
        }

        // GET: Catalog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null id");
                return BadRequest();
            }

            _logger.LogInformation("Loading catalog delete page for id={Id}", id);
            
            var catalogItem = await _service.FindCatalogItemAsync(id.Value);
            if (catalogItem == null)
            {
                _logger.LogWarning("Catalog item with id={Id} not found", id);
                return NotFound();
            }
            
            AddUriPlaceHolder(catalogItem);
            return View(catalogItem);
        }

        // POST: Catalog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Deleting catalog item with id={Id}", id);
            
            var catalogItem = await _service.FindCatalogItemAsync(id);
            if (catalogItem != null)
            {
                await _service.RemoveCatalogItemAsync(catalogItem);
                _logger.LogInformation("Successfully deleted catalog item with id={Id}", id);
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
}
