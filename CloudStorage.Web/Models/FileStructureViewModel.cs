namespace CloudStorage.Web.Models;

public class FileStructureViewModel
{
    public IEnumerable<FileViewModel> Files { get; set; }
    public IEnumerable<FileFolderViewModel> Folders { get; set; }
}