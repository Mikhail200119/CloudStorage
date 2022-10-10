using CloudStorage.DAL.Entities;

namespace CloudStorage.DAL.Repositories;

public interface IFilesRepository
{
    Task CreateAsync(FileDbModel file);
    void Update(FileDbModel file);
    void Delete(int id);
    Task<IEnumerable<FileDbModel>> GetAllFilesAsync(string email, bool trackEntities = false);
}