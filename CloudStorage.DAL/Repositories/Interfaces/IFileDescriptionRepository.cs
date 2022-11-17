using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL.Repositories.Interfaces;

public interface IFileDescriptionRepository
{
    Task CreateAsync(FileDescriptionDbModel fileDescription);
    Task<FileDescriptionDbModel?> GetByIdAsync(int id);
    void Update(FileDescriptionDbModel fileDescription);
    void Delete(int id);
    Task<IEnumerable<FileDescriptionDbModel>> GetAllFilesAsync(string email, bool trackEntities = false);
    Task<IEnumerable<string>> GetContentHashesAsync(string userEmail);
    Task<bool> ContentHashExistAsync(string contentHash);
}