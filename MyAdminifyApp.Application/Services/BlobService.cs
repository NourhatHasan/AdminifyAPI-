using Azure.Storage.Blobs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyAdminifyApp.Application.Interfaces;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "uploads";
    private readonly ILogger<BlobService> _logger;
    private readonly IMemoryCache _cache;
    public BlobService(IConfiguration configuration, ILogger<BlobService> logger, IMemoryCache cache)
    {
        _blobServiceClient = new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]);
        _logger = logger;
        _cache = cache;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
                _logger.LogInformation($"File '{fileName}' deleted successfully.");
                _cache.Remove(fileName);
            }
            else
            {
                _logger.LogWarning($"File '{fileName}' not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete file '{fileName}'.");
            throw;
        }

    }

    public async Task<bool> FileExistsAsync(string fileName)
    {
        if (_cache.TryGetValue(fileName, out bool exists))
        {
            _logger.LogInformation($"Cache hit for file '{fileName}' existence check.");
            return exists;
        }

        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            exists = await blobClient.ExistsAsync();

            _cache.Set(fileName, exists, TimeSpan.FromMinutes(5));
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error checking existence of file '{fileName}'.");
            throw;
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(fileName);
            _logger.LogInformation($"Uploading file '{fileName}' to Blob Storage.");

            await blobClient.UploadAsync(fileStream, overwrite: true);
            _logger.LogInformation($"File '{fileName}' uploaded successfully.");
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to upload file '{fileName}'.");
            throw;
        }
    }
}
