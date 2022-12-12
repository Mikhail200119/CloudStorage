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

    public async Task<FileDescriptionDbModel?> GetByIdAsync(int id)
    {
        return await Table
            .AsNoTracking()
            .SingleOrDefaultAsync(file => file.Id == id);
    }

    public async Task<IEnumerable<FileDescriptionDbModel>> GetAllFilesAsync(string email, bool trackEntities = false)
    {
        var files = trackEntities ? Table : Table.AsNoTracking();

        return await files
            .Where(file => file.UploadedBy == email)
            .ToListAsync();
    }

    public async Task<bool> ContentHashExist(string contentHash, string userEmail) => await Table.AnyAsync(file => file.ContentHash == contentHash && file.UploadedBy == userEmail);
    public async Task<bool> FileNameExist(string providedFileName, string userEmail) => await Table.AnyAsync(file => file.ProvidedName == providedFileName && file.UploadedBy == userEmail);
    public async Task<IEnumerable<FileDescriptionDbModel>> GetAllFiles(Predicate<FileDescriptionDbModel> searchOption, string email, bool trackEntities = false)
    {
        return await Table.Where(file => searchOption(file))
            .ToListAsync();
    }
}