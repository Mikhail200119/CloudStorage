using CloudStorage.BLL.Options;
using CloudStorage.BLL.Services.Interfaces;
using Microsoft.Extensions.Options;
using Xabe.FFmpeg;

namespace CloudStorage.BLL.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _storageOptions;

    public FileStorageService(IOptions<FileStorageOptions> fileStorageOptions)
    {
        _storageOptions = fileStorageOptions.Value;
    }

    public async Task UploadAsync(string fileName, byte[] data)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

        await using var file = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

        await file.WriteAsync(data);
    }

    public void Delete(string fileName)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public async Task<byte[]> GetAsync(string fileName)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

        await using var file = File.OpenRead(filePath);

        var data = new byte[file.Length];

        await file.ReadAsync(data);

        return data;
    }

    public async Task<byte[]> GetVideoThumbnailAsync(string fileName)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

        var outputFilePath = Path.Combine(_storageOptions.FilesDirectoryPath, $"{Guid.NewGuid()}.jpeg");

        FFmpeg.SetExecutablesPath(_storageOptions.FFmpegExecutablesPath);

        var conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(filePath, outputFilePath, TimeSpan.FromSeconds(0));
        await conversion.Start();

        var thumbnail = await GetAsync(outputFilePath);
        Delete(outputFilePath);

        return thumbnail;
    }
}