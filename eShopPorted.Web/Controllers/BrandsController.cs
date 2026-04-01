using eShopPorted.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShopPorted.Web.Controllers;

/// <summary>
/// API Controller for catalog brands
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly ICatalogService _service;
    private readonly ILogger<BrandsController> _logger;

    public BrandsController(ICatalogService service, ILogger<BrandsController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogInformation("Getting all catalog brands");
        
        var brands = _service.GetCatalogBrands();
        return Ok(brands);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation("Deleting brand with id={Id}", id);
        
        var brandToDelete = _service.GetCatalogBrands().FirstOrDefault(x => x.Id == id);
        if (brandToDelete == null)
        {
            return NotFound();
        }

        // Demo only - don't actually delete
        return Ok();
    }
}
