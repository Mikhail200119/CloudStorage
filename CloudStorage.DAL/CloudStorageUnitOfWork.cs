﻿using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Repositories;
using CloudStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL;

public class CloudStorageUnitOfWork : DbContext, ICloudStorageUnitOfWork
{
    private IFileDescriptionRepository? _fileDescriptionRepository;

    private DbSet<FileDescriptionDbModel> FileDescriptionTable { get; set; }

    public CloudStorageUnitOfWork(DbContextOptions<CloudStorageUnitOfWork> options) : base(options)
    {
    }

    public IFileDescriptionRepository FileDescription => _fileDescriptionRepository ??= new FileDescriptionRepository(this);

    public async Task SaveChangesAsync() => await base.SaveChangesAsync();
}