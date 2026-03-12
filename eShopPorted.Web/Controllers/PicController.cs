using Microsoft.AspNetCore.Mvc;

namespace eShopPorted.Web.Controllers
{
    /// <summary>
    /// Controller for serving catalog item pictures
    /// </summary>
    public class PicController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<PicController> _logger;

        public PicController(IWebHostEnvironment env, ILogger<PicController> logger)
        {
            _env = env;
            _logger = logger;
        }

        [HttpGet]
        [Route("Pics/{id}.png")]
        public IActionResult GetImage(int id)
        {
            _logger.LogInformation("Retrieving image for catalog item id={Id}", id);
            
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot, "Pics", $"{id}.png");

            if (!System.IO.File.Exists(path))
            {
                _logger.LogWarning("Image not found for catalog item id={Id}", id);
                return NotFound();
            }

            var imageFileStream = System.IO.File.OpenRead(path);
            return File(imageFileStream, "image/png");
        }
    }
}
