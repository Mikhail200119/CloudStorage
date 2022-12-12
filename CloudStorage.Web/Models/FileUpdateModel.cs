using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Web.Models;

public class FileUpdateModel
{
    public IFormFile FormFile { get; set; }

    public string UserName { get; set; }

    [MaxLength(20)]
    public string FolderName { get; set; }
}