using CloudStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL.Repositories;

public class FilesRepository : EfRepository<FileDbModel>, IFilesRepository
{
    public FilesRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<FileDbModel>> GetAllFilesAsync(string email, bool trackEntities = false)
    {
        var files = trackEntities ? _table : _table.AsNoTracking();

        return await files
            .Where(file => file.UserName == email)
            .ToListAsync();
    }
}