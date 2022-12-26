using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL.Repositories;

public class FileFolderRepository : EfRepository<FileFolderDbModel>, IFileFolderRepository
{
    public FileFolderRepository(DbContext context) : base(context)
    {
    }
}