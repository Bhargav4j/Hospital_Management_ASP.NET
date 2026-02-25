using Amazon.S3;
using Amazon.S3.Model;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Services;

public class S3StorageService : IS3StorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly ILogger<S3StorageService> _logger;

    public S3StorageService(IAmazonS3 s3Client, IConfiguration configuration, ILogger<S3StorageService> logger)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:S3:BucketName"]
            ?? throw new ArgumentNullException("AWS:S3:BucketName configuration is required");
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string? subfolder = null)
    {
        var key = string.IsNullOrEmpty(subfolder) ? fileName : $"{subfolder}/{fileName}";

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        _logger.LogInformation("Uploading file {Key} to S3 bucket {Bucket}", key, _bucketName);
        await _s3Client.PutObjectAsync(request);
        _logger.LogInformation("Successfully uploaded file {Key} to S3", key);

        return key;
    }

    public async Task<Stream> DownloadFileAsync(string key)
    {
        _logger.LogInformation("Downloading file {Key} from S3 bucket {Bucket}", key, _bucketName);

        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _s3Client.GetObjectAsync(request);
        var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<bool> DeleteFileAsync(string key)
    {
        _logger.LogInformation("Deleting file {Key} from S3 bucket {Bucket}", key, _bucketName);

        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _s3Client.DeleteObjectAsync(request);
        return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
    }

    public async Task<string> GetPreSignedUrlAsync(string key, int expirationInMinutes = 60)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
            Verb = HttpVerb.GET
        };

        return await Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public async Task<IEnumerable<string>> ListFilesAsync(string? prefix = null)
    {
        _logger.LogInformation("Listing files in S3 bucket {Bucket} with prefix {Prefix}", _bucketName, prefix);

        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = prefix
        };

        var response = await _s3Client.ListObjectsV2Async(request);
        return response.S3Objects.Select(obj => obj.Key);
    }
}
