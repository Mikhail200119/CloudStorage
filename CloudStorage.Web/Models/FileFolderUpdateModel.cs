namespace CloudStorage.Web.Models;

public class FileFolderUpdateModel : FileFolderBase
{
    public int Id { get; set; }
    public IEnumerable<FileUpdateModel> Files { get; set; }
}