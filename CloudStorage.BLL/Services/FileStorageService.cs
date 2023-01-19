using CloudStorage.BLL.Options;
using CloudStorage.BLL.Services.Interfaces;
using Microsoft.Extensions.Options;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

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

    public async Task UploadStreamAsync(string fileName, Stream data)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

        await using var fileStream = File.Create(filePath);
        data.Seek(0, SeekOrigin.Begin);
        await data.CopyToAsync(fileStream);
    }

    public async Task UploadRangeAsync(IEnumerable<(string FileName, Stream Content)> files)
    {
        var uploadFileTasks = files.Select(file =>
        {
            var (fileName, content) = file;
            var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

            return Task.Run(async () =>
            {
                await using var fileStream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                content.Seek(0, SeekOrigin.Begin);
                await content.CopyToAsync(fileStream);
            });
        });

        await Task
            .WhenAll(uploadFileTasks)
            .ConfigureAwait(false);
    }

    public void Delete(string fileName)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public async Task DeleteRange(params string[] fileNames)
    {
        foreach (var fileName in fileNames)
        {
            await Task.Run(() =>
            {
                var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            });
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

        var file = File.Open(filePath, new FileStreamOptions
        {
            Mode = FileMode.Open,
            Access = FileAccess.Read,
            Share = FileShare.Delete | FileShare.Read
        });

        return file;
    }

    public async Task<byte[]> GetVideoThumbnailAsync(string fileName)
    {
        var filePath = Path.Combine(_storageOptions.FilesDirectoryPath, fileName);
        var outputFilePath = Path.Combine(_storageOptions.FilesDirectoryPath, $"{Guid.NewGuid()}.jpeg");
        
        await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full, _storageOptions.FFmpegExecutablesPath);
        FFmpeg.SetExecutablesPath(_storageOptions.FFmpegExecutablesPath, "ffmpeg.exe", "ffprobe.exe");

        var conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(filePath, outputFilePath, TimeSpan.FromSeconds(0));
        await conversion.Start();

        var thumbnail = await GetFileAsync(outputFilePath, decrypt: false);
        Delete(outputFilePath);

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