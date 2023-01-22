using CloudStorage.DAL.Repositories.Interfaces;

namespace CloudStorage.DAL;

public interface ICloudStorageUnitOfWork
{
    IFileDescriptionRepository FileDescription { get; }
    IThumbnailInfoRepository ThumbnailInfo { get; }

    Task SaveChangesAsync();
}