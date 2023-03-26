using CloudStorage.BLL.Models;

namespace CloudStorage.BLL.Services.Interfaces;

public interface ICloudStorageManager
{
    Task<IEnumerable<FileDescription>> CreateAsync(IEnumerable<FileCreateData> files);
    Task<(Stream Data, string ContentType)> GetFileStreamAndContentTypeAsync(int fileId);
    Task<FileDescription> UpdateAsync(FileUpdateData existingFile);
    Task DeleteAsync(int id);
    Task DeleteRangeAsync(IEnumerable<int> ids);
    Task<IEnumerable<FileDescription>> GetAllFilesAsync();
    Task RenameFileAsync(int id, string newName);
}