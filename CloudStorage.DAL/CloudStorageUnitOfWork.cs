using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL;

public class CloudStorageUnitOfWork : DbContext, ICloudStorageUnitOfWork
{
    private IFilesRepository? _filesRepository;

    private DbSet<FileDbModel> FilesTable { get; set; }

    public CloudStorageUnitOfWork(DbContextOptions<CloudStorageUnitOfWork> options) : base(options)
    {
    }

    public IFilesRepository Files => _filesRepository ??= new FilesRepository(this);

    public async Task SaveChangesAsync() => await base.SaveChangesAsync();
}