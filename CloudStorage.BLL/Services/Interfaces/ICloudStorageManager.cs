using CloudStorage.BLL.Models;

namespace CloudStorage.BLL.Services.Interfaces;

public interface ICloudStorageManager
{
    Task<FileDescription> CreateAsync(FileCreateData newFile);
    Task<IEnumerable<FileDescription>> CreateAsync(IEnumerable<FileCreateData> files);
    Task<byte[]> GetFileContentAsync(int fileId);
    Task<string> GetUniqueNameAsync(int fileId);
    Task<Stream> GetFileStreamAsync(int fileId);
    Task<FileDescription> UpdateAsync(FileUpdateData existingFile);
    Task DeleteAsync(int id);
    Task<IEnumerable<FileDescription>> GetAllFilesAsync();
    Task RenameFileAsync(int id, string newName);
}