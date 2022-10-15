using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL.Repositories;

public class FileContentRepository : EfRepository<FileContentDbModel>, IFileContentRepository
{
    public FileContentRepository(DbContext context) : base(context)
    {
    }

    public async Task<FileContentDbModel?> GetFileContentByIdAsync(int fileId) => 
        await Table.SingleOrDefaultAsync(file => file.FileDescriptionId == fileId);
}