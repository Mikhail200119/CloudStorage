namespace CloudStorage.Web.Services.Interfaces;

public interface IWordToPdfConverter
{
    Task<Stream> GetConvertedFile(Stream data);
}