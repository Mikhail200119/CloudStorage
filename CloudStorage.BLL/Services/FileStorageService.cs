using CloudStorage.BLL.Options;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.Common.Extensions;
using Microsoft.Extensions.Options;
using Xabe.FFmpeg;

namespace CloudStorage.BLL.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IAesEncryptor _aesEncryptor;
    private readonly FileStorageOptions _storageOptions;

    public FileStorageService(IAesEncryptor aesEncryptor, IOptions<FileStorageOptions> fileStorageOptions)
    {
        _aesEncryptor = aesEncryptor;
        _storageOptions = fileStorageOptions.Value;
    }

    public async Task UploadAsync(string fileName, byte[] data)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

        await UploadFileAsync(filePath, data, encrypt: false);
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

        return await GetFileAsync(filePath);
    }

    public async Task<Stream> GetStreamAsync(string fileName)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

        var file = File.OpenRead(filePath);

        return file;
    }

    public async Task<byte[]> GetVideoThumbnailAsync(string fileName)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);
        var outputFilePath = Path.Combine(_storageOptions.FilesDirectoryPath, $"{Guid.NewGuid()}.jpeg");

        //var decryptedFile = await GetFileAsync(filePath);
        var decryptedFile = await GetStreamAsync(fileName);

        var tmpFileName = Guid.NewGuid().ToString();
        var tmpFilePath = Path.Combine(_storageOptions.FilesDirectoryPath, tmpFileName);
        await UploadFileAsync(tmpFilePath, decryptedFile.ToArray(), encrypt: false);

        FFmpeg.SetExecutablesPath(_storageOptions.FFmpegExecutablesPath);

        var conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(tmpFilePath, outputFilePath, TimeSpan.FromSeconds(0));
        await conversion.Start();

        var thumbnail = await GetFileAsync(outputFilePath, decrypt: false);
        Delete(outputFilePath);
        Delete(tmpFileName);

        return thumbnail;
    }

    private async Task UploadFileAsync(string filePath, byte[] data, bool encrypt = true)
    {
        if (encrypt)
        {
            data = await _aesEncryptor.EncryptAsync(data);
        }

        await using var file = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

        await file.WriteAsync(data);
    }

    private async Task<byte[]> GetFileAsync(string filePath, bool decrypt = true)
    {
        await using var file = File.OpenRead(filePath);

        var data = new byte[file.Length];

        await file.ReadAsync(data);

        return decrypt ? await _aesEncryptor.DecryptAsync(data) : data;
    }
}