namespace CloudStorage.BLL.Services.Interfaces;

public interface IFileStorageService
{
    Task UploadAsync(string fileName, byte[] data);
    Task UploadStreamAsync(string fileName, Stream data);
    void Delete(string fileName);
    Task<byte[]> GetAsync(string fileName);
    Task<Stream> GetStreamAsync(string fileName);
    Task<byte[]> GetVideoThumbnailAsync(string fileName);
}