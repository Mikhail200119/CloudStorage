namespace CloudStorage.Web.Models;

public class FileFolderViewModel : FileFolderBase
{
    public int Id { get; set; }
    public IEnumerable<FileViewModel> Files { get; set; }
}