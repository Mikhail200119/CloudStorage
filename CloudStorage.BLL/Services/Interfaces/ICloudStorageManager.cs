using CloudStorage.BLL.Models;

namespace CloudStorage.BLL.Services.Interfaces;

public interface ICloudStorageManager
{
    Task<IEnumerable<FileDescription>> CreateAsync(IEnumerable<FileCreateData> files);
    Task<(Stream Data, string ContentType)> GetFileStreamAndContentTypeAsync(int fileId);
    Task<(Stream Data, string ContentType)> GetThumbnailStreamAndContentTypeAsync(int thumbId);
    Task DeleteRangeAsync(IEnumerable<int> ids);
    Task<IEnumerable<FileDescription>> GetAllFilesAsync();
    Task<IEnumerable<FileDescription>> SearchFilesAsync(FileSearchData fileSearchData);
    Task<IEnumerable<string>> GetArchiveFileNamesAsync(int fileId);
    Task<(string? contentType, Stream? data)> GetArchiveFileContent(int fileId, string archiveFilePath);
    Task RenameFileAsync(int id, string newName);
}