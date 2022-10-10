using CloudStorage.DAL.Repositories;

namespace CloudStorage.DAL;

public interface ICloudStorageUnitOfWork
{
    IFilesRepository Files { get; }

    Task SaveChangesAsync();
}