using CloudStorage.DAL.Entities;

namespace CloudStorage.DAL.Repositories.Interfaces;

public interface IFileFolderRepository
{
    Task CreateAsync(FileFolderDbModel folder);
    void Delete(int id);
    void Update(FileFolderDbModel folder);
    Task<FileFolderDbModel> GetById(int id);
    Task<IEnumerable<FileFolderDbModel>> GetAllFoldersByIdsAsync(IEnumerable<int> ids);

    Task<int> GetRootFolderIdAsync();

    Task<IEnumerable<FileFolderDbModel>> GetAllFolders(string userEmail);
}