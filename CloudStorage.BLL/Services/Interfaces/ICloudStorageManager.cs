using CloudStorage.BLL.Models;
using CloudStorage.DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.BLL.Services.Interfaces;

public interface ICloudStorageManager
{
    Task<FileDescription> CreateAsync(FileCreateData newFile);

    Task<FileDescription> GetByIdAsync(int id);

    Task<byte[]> GetContentByFileDescriptionIdAsync(int id);

    Task<FileDescription> UpdateAsync(FileUpdateData existingFile);
    Task DeleteAsync(int id);

    Task<IEnumerable<FileDescription>> GetAllFilesAsync();
}