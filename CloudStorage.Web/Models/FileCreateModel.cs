using System.ComponentModel.DataAnnotations;

namespace CloudStorage.Web.Models;

public class FileCreateModel
{
    [Required]
    public IFormFile FormFile { get; set; }
    public int FolderId { get; set; }
}