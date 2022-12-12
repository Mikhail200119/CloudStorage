using System.ComponentModel.DataAnnotations;
using CloudStorage.Web.Attributes;

namespace CloudStorage.Web.Models;

public class FileCreateModel
{
    [Required]
    [FileContentRequired]
    public IFormFile FormFile { get; set; }

    [MaxLength(20)]
    public string FolderName { get; set; }
}