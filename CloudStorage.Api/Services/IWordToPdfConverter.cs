namespace CloudStorage.Api.Services;

public interface IWordToPdfConverter
{
    Task<Stream> GetPdfFromWordAsync(int fileId);
}