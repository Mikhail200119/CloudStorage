using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL;

public class CloudStorageUnitOfWork : DbContext, ICloudStorageUnitOfWork
{
    private IUsersRepository? _usersRepository;
    private IFilesRepository? _filesRepository;

    private DbSet<FileDbModel> FilesTable { get; set; }
    private DbSet<UserDbModel> UsersTable { get; set; }

    public CloudStorageUnitOfWork(DbContextOptions<CloudStorageUnitOfWork> options) : base(options)
    {
    }

    public IUsersRepository Users => _usersRepository ??= new UsersRepository(this);
    public IFilesRepository Files => _filesRepository ??= new FilesRepository(this);

    public async Task SaveChangesAsync() => await base.SaveChangesAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<FileDbModel>()
            .HasKey(e => e.Id);
    }
}