using CloudStorage.DAL.Entities;

namespace CloudStorage.DAL.Repositories.Interfaces;

public interface IFileDescriptionRepository
{
    Task CreateAsync(FileDescriptionDbModel fileDescription);
    Task CreateRangeAsync(IEnumerable<FileDescriptionDbModel> filesDescriptions);
    Task<bool> ContentHashesExistAsync(string userMail, params string[] contentHashes);
    Task<FileDescriptionDbModel?> GetByIdAsync(int id);
    void Update(FileDescriptionDbModel fileDescription);
    void Delete(int id);
    Task<IEnumerable<FileDescriptionDbModel>> GetAllFilesAsync(string email, bool trackEntities = false);
    Task<bool> ContentHashExist(string contentHash, string userEmail);
    Task<bool> FileNameExist(string providedFileName, string userEmail);
    Task<bool> FileNamesExist(string userMail, params string[] names);
}