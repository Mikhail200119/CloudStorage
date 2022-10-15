using CloudStorage.DAL.Entities;

namespace CloudStorage.DAL.Repositories.Interfaces;

public interface IFileContentRepository
{
    Task<FileContentDbModel?> GetFileContentByIdAsync(int fileId);
}