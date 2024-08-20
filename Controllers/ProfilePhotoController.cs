using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CrudApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilePhotoController : ControllerBase
    {
        private readonly ILogger<ProfilePhotoController> _logger;

        public ProfilePhotoController( ILogger<ProfilePhotoController> logger)
        {
            _logger = logger;
        }

        // GET: api/ProfilePhoto/{fileName}
        [HttpGet("{fileName}")]
        public IActionResult GetProfilePhoto(string fileName)
        {
            _logger.LogInformation($"Requested file name: {fileName}");

            var basePath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(basePath, "Assets", "uploads", fileName);

            _logger.LogInformation($"Looking for file at: {filePath}");

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogWarning($"File not found: {filePath}");
                return NotFound();
            }

            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg"); // Adjust MIME type as needed
        }

        // POST: api/ProfilePhoto
        [HttpPost]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded.");
                return BadRequest("No file uploaded.");
            }

            var basePath = Directory.GetCurrentDirectory();
            var filesFolderPath = Path.Combine(basePath, "Assets", "uploads");
            if (!Directory.Exists(filesFolderPath))
            {
                Directory.CreateDirectory(filesFolderPath);
                _logger.LogInformation($"Created directory: {filesFolderPath}");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(filesFolderPath, fileName);

            _logger.LogInformation($"Uploading file to: {filePath}");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Ok(new { FileName = fileName });
        }

        // DELETE: api/ProfilePhoto/{fileName}
        [HttpDelete("{fileName}")]
        public IActionResult DeleteProfilePhoto(string fileName)
        {
            _logger.LogInformation($"Requested to delete file: {fileName}");

            var basePath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(basePath, "Assets", "uploads", fileName);

            _logger.LogInformation($"Looking for file at: {filePath}");

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogWarning($"File not found: {filePath}");
                return NotFound();
            }

            System.IO.File.Delete(filePath);
            _logger.LogInformation($"Deleted file at: {filePath}");

            return NoContent();
        }
    }
}
