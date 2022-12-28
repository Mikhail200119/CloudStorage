namespace CloudStorage.Web.Models;

public abstract class FileFolderBase
{ 
    public string Name { get; set; }
    public int? ParentFolderId { get; set; }
}