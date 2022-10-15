using System.ComponentModel.DataAnnotations.Schema;
using CloudStorage.DAL.Entities.Interfaces;

namespace CloudStorage.DAL.Entities;

[Table("FileContent")]
public class FileContentDbModel : IEntity
{
    [Column("FileContentId")]
    public int Id { get; set; }

    public int FileDescriptionId { get; set; }

    public FileDescriptionDbModel FileDescription { get; set; }
    
    public byte[] Content { get; set; }
}