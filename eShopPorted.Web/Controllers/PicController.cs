using eShopPorted.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShopPorted.Web.Controllers;

/// <summary>
/// Controller for serving catalog item pictures
/// </summary>
public class PicController : Controller
{
    private readonly ICatalogService _service;
    private readonly ILogger<PicController> _logger;
    private readonly IWebHostEnvironment _environment;

    public PicController(ICatalogService service, ILogger<PicController> logger, IWebHostEnvironment environment)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    // GET: Pic/5.png or /items/5/pic
    [HttpGet]
    [Route("items/{catalogItemId:int}/pic")]
    public IActionResult Index(int catalogItemId)
    {
        _logger.LogInformation("Loading picture for catalog item id={CatalogItemId}", catalogItemId);

        if (catalogItemId <= 0)
        {
            return BadRequest();
        }

        var item = _service.FindCatalogItem(catalogItemId);

        if (item != null)
        {
            var webRoot = _environment.WebRootPath;
            var path = Path.Combine(webRoot, "Pics", item.PictureFileName);

            if (!System.IO.File.Exists(path))
            {
                _logger.LogWarning("Picture file not found: {Path}", path);
                return NotFound();
            }

            string imageFileExtension = Path.GetExtension(item.PictureFileName);
            string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

            var buffer = System.IO.File.ReadAllBytes(path);

            return File(buffer, mimetype);
        }

        return NotFound();
    }

    private string GetImageMimeTypeFromImageFileExtension(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            ".wmf" => "image/wmf",
            ".jp2" => "image/jp2",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
    }
}
