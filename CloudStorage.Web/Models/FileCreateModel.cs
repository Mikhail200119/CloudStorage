using System.ComponentModel.DataAnnotations;
using CloudStorage.Web.Attributes;

namespace CloudStorage.Web.Models;

public class FileCreateModel
{
    [Required]
    [FileContentRequired]
    public IEnumerable<IFormFile> FormFiles { get; set; }
}