﻿using CloudStorage.BLL.Models;

namespace CloudStorage.BLL.Services.Interfaces;

public interface ICloudStorageManager
{
    Task<FileDescription> CreateAsync(FileCreateData newFile);
    Task<byte[]> GetFileContentAsync(int fileId);
    Task<FileDescription> UpdateAsync(FileUpdateData existingFile);
    Task DeleteAsync(int id);
    Task<IEnumerable<FileDescription>> GetAllFilesAsync();
}