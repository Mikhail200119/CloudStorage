namespace CloudStorage.Web.Helpers;

public static class FileContentTypes
{
    public static class Text
    {
        private const string ContentTypeName = "text";

        public const string Plain = $"{ContentTypeName}/plain";
    }

    public static class Image
    {
        private const string ContentTypeName = "image";

        public const string Png = $"{ContentTypeName}/png";
    }

    public static class Video
    {
        private const string ContentTypeName = "video";

        public const string Mp4 = $"{ContentTypeName}/mp4";
    }
    
    public static class Application
    {
        private const string ContentTypeName = "application";

        public const string Pdf = $"{ContentTypeName}/pdf";
        public const string MsWord = $"{ContentTypeName}/vnd.openxmlformats-officedocument.wordprocessingml.document";
    }
}