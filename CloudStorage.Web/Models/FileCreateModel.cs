using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Web.Models;

public class FileCreateModel
{
    [Required]
    public IFormFile FormFile { get; set; }

    [MaxLength(20)]
    public string FolderName { get; set; }
}