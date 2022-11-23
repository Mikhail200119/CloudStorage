namespace CloudStorage.Web.Models;

public class FileFolderCreateModel : FileFolderBase
{
    public IEnumerable<FileCreateModel> Files { get; set; }
}