using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL.Repositories;

public class FileFolderRepository : EfRepository<FileFolderDbModel>, IFileFolderRepository
{
    public FileFolderRepository(DbContext context) : base(context)
    {
    }

    public async Task<FileFolderDbModel> GetById(int id) => await Table.SingleAsync(folder => folder.Id == id);

    public async Task<IEnumerable<FileFolderDbModel>> GetAllFoldersByIdsAsync(IEnumerable<int> ids)
    {
        var folders = new List<FileFolderDbModel>();

        foreach (var id in ids)
        {
            var item = await Table.SingleAsync(folder => folder.Id == id);
            folders.Add(item);
        }

        return folders;
    }

    public async Task<int> GetRootFolderIdAsync()
    {
        var table = Table.AsNoTracking();

        var rootFolder = await table.FirstAsync(folder => folder.ParentFolderId == null);

        return rootFolder.Id;
    }

    public async Task<IEnumerable<FileFolderDbModel>> GetAllFolders(string userEmail)
    {
        var table = Table.AsNoTracking();

        return await table
            .Where(folder => folder.UserEmail == userEmail)
            .Where(folder => folder.UserEmail == null)
            .ToListAsync();
    }
}