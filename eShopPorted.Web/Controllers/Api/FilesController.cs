using Microsoft.AspNetCore.Mvc;

namespace eShopPorted.Web.Controllers.Api
{
    /// <summary>
    /// API controller for file operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IWebHostEnvironment env, ILogger<FilesController> logger)
        {
            _env = env;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File upload attempted with no file");
                return BadRequest("No file uploaded");
            }

            _logger.LogInformation("Uploading file: {FileName}, Size: {FileSize}", file.FileName, file.Length);

            var uploadsFolder = Path.Combine(_env.WebRootPath, "Pics");
            
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            _logger.LogInformation("File uploaded successfully: {FilePath}", filePath);

            return Ok(new { fileName = uniqueFileName });
        }
    }
}
