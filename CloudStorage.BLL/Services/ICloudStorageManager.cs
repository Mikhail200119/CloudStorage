using CloudStorage.BLL.Models;
using CloudStorage.DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = CloudStorage.BLL.Models.File;

namespace CloudStorage.BLL.Services;

public interface ICloudStorageManager
{
    Task<File> CreateAsync(FileCreateData newFile);

    Task<File> GetByIdAsync(int id);

    Task<byte[]> GetContentByFileDescriptionIdAsync(int id);

    Task<File> UpdateAsync(FileUpdateData existingFile);
    Task DeleteAsync(int id);

    Task<IEnumerable<File>> GetAllFilesAsync();
}