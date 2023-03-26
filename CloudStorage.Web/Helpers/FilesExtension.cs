namespace CloudStorage.Web.Helpers;

public static class FilesExtension
{
    private static readonly IEnumerable<string> MsWordExtensions = new List<string>
    {
        "doc",
        "docx"
    };

    private static readonly IEnumerable<string> VideoExtensions = new List<string>
    {
        "mp4",
        "avi"
    };

    private static readonly IEnumerable<string> ArchiveExtensions = new List<string>
    {
        "zip",
        "rar",
        "jar"
    };

    private static readonly IEnumerable<string> CodeExtensions = new List<string>
    {
        "cs",
        "java",
        "xml",
        "json",
        "js",
        "c",
        "cpp"
    };

    public static bool IsWord(string extension) => MsWordExtensions.Contains(extension);

    public static bool IsVideo(string extension) => VideoExtensions.Contains(extension);
    
    public static bool IsArchive(string extension) => ArchiveExtensions.Contains(extension);
    
    public static bool IsCode(string extension) => CodeExtensions.Contains(extension);


    public static class Text
    {
        public const string Txt = "txt";
        public const string Doc = "doc";
        public const string Docx = "docx";
        public const string Pdf = "pdf";
    }
    
    public static class Video
    {
        public const string Mp4 = "mp4";
        public const string Avi = "avi";
    }

    public static class Image
    {
        public const string Png = "png";
        public const string Jpg = "jpg";
        public const string Jpeg = "jpeg";
    }

    public static class Archive
    {
        public const string Zip = "zip";
        public const string Rar = "rar";
        public const string Jar = "jar";
    }

    public static class Code
    {
        public const string Cs = "cs";
        public const string Java = "java";
        public const string Xml = "xml";
        public const string Json = "json";
        public const string Js = "js";
        public const string C = "c";
        public const string Cpp = "cpp";   
    }
}