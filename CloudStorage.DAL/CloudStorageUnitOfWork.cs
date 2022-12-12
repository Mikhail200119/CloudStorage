using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories;
using CloudStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL;

public class CloudStorageUnitOfWork : DbContext, ICloudStorageUnitOfWork
{
    private IFileDescriptionRepository? _fileDescriptionRepository;
    private IFileFolderRepository? _fileFolderRepository;

    private DbSet<FileDescriptionDbModel> FileDescriptionTable { get; set; }
    private DbSet<FileFolderDbModel> FileFolderTable { get; set; }

    public CloudStorageUnitOfWork(DbContextOptions<CloudStorageUnitOfWork> options) : base(options)
    {
    }

    public IFileDescriptionRepository FileDescription => _fileDescriptionRepository ??= new FileDescriptionRepository(this);
    public IFileFolderRepository FileFolder => _fileFolderRepository ?? new FileFolderRepository(this);

    public async Task SaveChangesAsync() => await base.SaveChangesAsync();
}