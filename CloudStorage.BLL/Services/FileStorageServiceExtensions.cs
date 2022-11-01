using CloudStorage.BLL.Services.Interfaces;
using ImageMagick;

namespace CloudStorage.BLL.Services;

internal static class FileStorageServiceExtensions
{
    public static byte[] CompressImage(this IFileStorageService _, byte[] imageData)
    {
        using var image = new MagickImage(imageData);
        image.Minify();
        image.Quality = 75;

        return image.ToByteArray();
    }
}