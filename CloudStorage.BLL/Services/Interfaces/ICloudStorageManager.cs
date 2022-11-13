using CloudStorage.BLL.Models;

namespace CloudStorage.BLL.Services.Interfaces;

public interface ICloudStorageManager
{
    Task<FileDescription> CreateFileAsync(FileCreateData newFile);
    Task<byte[]> GetFileContentAsync(int fileId);
    Task<FileDescription> UpdateFileAsync(FileUpdateData existingFile);
    Task DeleteFileAsync(int id);
    Task<IEnumerable<FileDescription>> GetAllFilesAsync();

    Task CreateFolderAsync(FileFolderCreateData folder);
    Task UpdateFolderAsync(FileFolderUpdateData folder);
    Task DeleteFolderAsync(int id);
}