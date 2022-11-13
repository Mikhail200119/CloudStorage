namespace CloudStorage.Web.Models;

public class FileUpdateModel
{
    public IFormFile FormFile { get; set; }
    public int FolderId { get; set; }
}