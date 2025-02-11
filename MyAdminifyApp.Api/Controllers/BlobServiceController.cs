using Microsoft.AspNetCore.Mvc;
using MyAdminifyApp.Api.DTOs;
using MyAdminifyApp.Application.Interfaces;

namespace MyAdminifyApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class BlobServiceController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly ILogger<BlobService> _logger;
        private static readonly string[] _allowedExtensions = { ".jpg", ".png", ".pdf" };

        public BlobServiceController(IBlobService blobService, ILogger<BlobService> logger)
        {
            _blobService = blobService;
            _logger = logger;
        }
        [HttpPost("upload")]
      
        public async Task<IActionResult> Upload([FromForm] IFormFile file)

        {
          
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Unsupported file type. Allowed types are .jpg, .png, .pdf.");
            }

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";


            try
            {
             

                using var stream = file.OpenReadStream();
                var fileUrl = await _blobService.UploadFileAsync(stream, uniqueFileName);
                var response = new FileUploadResponse
                {
                    Url = fileUrl,
                    UploadedAt = DateTime.UtcNow
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file.");
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }

        [HttpDelete("{fileName}")]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _blobService.DeleteFileAsync(fileName);
            return NoContent();
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> FileExists(string fileName)
        {
            bool exists = await _blobService.FileExistsAsync(fileName);
            return Ok(new { FileExists = exists });
        }
    }
}
