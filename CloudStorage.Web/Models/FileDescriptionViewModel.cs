namespace CloudStorage.Web.Models;

public class FileDescriptionViewModel
{
    public IEnumerable<FileViewModel> Files { get; set; }
    public IEnumerable<FileFolderViewModel> Folders { get; set; }
}