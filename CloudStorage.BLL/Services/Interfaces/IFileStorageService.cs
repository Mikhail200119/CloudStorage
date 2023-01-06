namespace CloudStorage.BLL.Services.Interfaces;

public interface IFileStorageService
{
    Task UploadStreamAsync(string fileName, Stream data);
    Task UploadRangeAsync(IEnumerable<(string FileName, Stream Content)> files);
    void Delete(string fileName);
    Task DeleteRange(params string[] fileNames);
    Task<byte[]> GetAsync(string fileName);
    Task<Stream> GetStreamAsync(string fileName);
    Task<byte[]> GetVideoThumbnailAsync(string fileName);
}