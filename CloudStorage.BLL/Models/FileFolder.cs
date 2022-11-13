namespace CloudStorage.BLL.Models;

public class FileFolder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentFolderId { get; set; }
}