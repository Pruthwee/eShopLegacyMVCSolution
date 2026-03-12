using eShopPorted.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShopPorted.Web.Controllers.Api
{
    /// <summary>
    /// API controller for catalog brands
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly ICatalogService _service;
        private readonly ILogger<BrandsController> _logger;

        public BrandsController(ICatalogService service, ILogger<BrandsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Retrieving all catalog brands");
            
            var brands = await _service.GetCatalogBrandsAsync();
            return Ok(brands);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete request for brand id={Id}", id);
            
            var brands = await _service.GetCatalogBrandsAsync();
            var brandToDelete = brands.FirstOrDefault(x => x.Id == id);
            
            if (brandToDelete == null)
            {
                _logger.LogWarning("Brand with id={Id} not found", id);
                return NotFound();
            }

            // Demo only - don't actually delete
            _logger.LogInformation("Brand delete operation (demo mode - not actually deleted)");
            return Ok();
        }
    }
}
