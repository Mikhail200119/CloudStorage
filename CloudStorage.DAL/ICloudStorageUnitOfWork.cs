using CloudStorage.DAL.Repositories.Interfaces;

namespace CloudStorage.DAL;

public interface ICloudStorageUnitOfWork
{
    IFileDescriptionRepository FileDescription { get; }
    IFileContentRepository FileContent { get; }

    Task SaveChangesAsync();
}