using System.IO.Compression;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services.Interfaces;

namespace CloudStorage.Web.Extensions;

public static class CloudStorageManagerExtensions
{
    public static async Task<IEnumerable<FileDescription>> GetFilesFromArchive(this ICloudStorageManager cloudStorageManager, int fileId)
    {
        var (data, _) = await cloudStorageManager.GetFileStreamAndContentTypeAsync(fileId);

        using var zipArchive = new ZipArchive(data);

        return zipArchive.Entries
            .Select(e => new FileDescription
            {
                ProvidedName = e.FullName,
                Extension = Path.GetExtension(e.FullName)
            });
    }

    public static async Task<Stream?> GetArchiveEntryContent(this ICloudStorageManager cloudStorageManager, int archiveId, string entryName)
    {
        var (data, _) = await cloudStorageManager.GetFileStreamAndContentTypeAsync(archiveId);

        using var zipArchive = new ZipArchive(data);

        return zipArchive.Entries.SingleOrDefault(e => e.FullName == entryName)?.Open();
    }
}