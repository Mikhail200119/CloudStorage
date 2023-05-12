using CloudStorage.Api.Exceptions;
using CloudStorage.Api.Options;
using CloudStorage.BLL.Options;
using CloudStorage.BLL.Services.Interfaces;
using ConvertApiDotNet;
using Microsoft.Extensions.Options;

namespace CloudStorage.Api.Services;

public class WordToPdfConverter : IWordToPdfConverter
{
    private readonly ICloudStorageManager _cloudStorageManager;
    private readonly PdfConvertOptions _pdfConvertOptions;

    public WordToPdfConverter(ICloudStorageManager cloudStorageManager, IOptions<FileStorageOptions> fileStorageOptions, IOptions<PdfConvertOptions> pdfConvertOptions)
    {
        _cloudStorageManager = cloudStorageManager;
        _pdfConvertOptions = pdfConvertOptions.Value;
    }

    public async Task<Stream> GetPdfFromWordAsync(int fileId)
    {
        var fileDescription = await _cloudStorageManager.GetFileDescriptionByIdAsync(fileId);

        if (fileDescription is null)
        {
            return Stream.Null;
        }

        if (fileDescription.Extension is not "doc" and not "docx")
        {
            throw new PdfConvertException($"Cannot convert file from {fileDescription.Extension} to pdf");
        }
        
        var (data, _, _) = await _cloudStorageManager.GetFileStreamAsync(fileId);

        var convertApi = new ConvertApi(_pdfConvertOptions.ApiSecretKey);
        var convert = await convertApi.ConvertAsync(fileDescription.Extension, "pdf",
            new ConvertApiFileParam(data, fileDescription.ProvidedName)
        );

        return await convert.Files.First().FileStreamAsync();
    }
}