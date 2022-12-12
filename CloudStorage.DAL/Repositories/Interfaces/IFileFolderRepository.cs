using CloudStorage.DAL.Entities;

namespace CloudStorage.DAL.Repositories.Interfaces;

public interface IFileFolderRepository
{
    Task CreateAsync(FileFolderDbModel folder);
    void Delete(FileFolderDbModel folder);
}