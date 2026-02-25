namespace ClinicManagement.Domain.Interfaces.Services;

public interface IS3StorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string? subfolder = null);
    Task<Stream> DownloadFileAsync(string key);
    Task<bool> DeleteFileAsync(string key);
    Task<string> GetPreSignedUrlAsync(string key, int expirationInMinutes = 60);
    Task<IEnumerable<string>> ListFilesAsync(string? prefix = null);
}
