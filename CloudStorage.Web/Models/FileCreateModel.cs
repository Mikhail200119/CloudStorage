namespace CloudStorage.Web.Models;

public class FileCreateModel
{
    public IFormFile FormFile { get; set; }

    public string UserName { get; set; }
}