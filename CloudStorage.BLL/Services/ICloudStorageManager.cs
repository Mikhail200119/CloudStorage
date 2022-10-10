using CloudStorage.BLL.Models;
using File = CloudStorage.BLL.Models.File;

namespace CloudStorage.BLL.Services;

public interface ICloudStorageManager
{
    Task<File> CreateAsync(FileCreateData newFile);
    Task<File> UpdateAsync(FileUpdateData existingFile);
    void Delete(int id);

    Task<IEnumerable<File>> GetAllFiles();
}