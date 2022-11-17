using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL.Repositories;

public class FileDescriptionRepository : EfRepository<FileDescriptionDbModel>, IFileDescriptionRepository
{
    public FileDescriptionRepository(DbContext context) : base(context)
    {
    }

    public override async Task CreateAsync(FileDescriptionDbModel entity)
    {
        Context.Database.SetCommandTimeout(200);

        await base.CreateAsync(entity);
    }

    public async Task<FileDescriptionDbModel?> GetByIdAsync(int id) => await Table.SingleOrDefaultAsync(file => file.Id == id);

    public async Task<IEnumerable<FileDescriptionDbModel>> GetAllFilesAsync(string email, bool trackEntities = false)
    {
        var files = trackEntities ? Table : Table.AsNoTracking();

        return await files
            .Where(file => file.UploadedBy == email)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetContentHashesAsync(string userEmail) =>
        await Table
            .Where(file => file.UploadedBy == userEmail)
            .Select(file => file.ContentHash)
            .ToListAsync();
}