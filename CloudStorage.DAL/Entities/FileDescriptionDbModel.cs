using System.ComponentModel.DataAnnotations.Schema;
using CloudStorage.DAL.Entities.Interfaces;

namespace CloudStorage.DAL.Entities;

[Table("FileDescription")]
public class FileDescriptionDbModel : IEntity
{
    [Column("FileDescriptionId")]
    public int Id { get; set; }

    public string ProvidedName { get; set; }

    public string UniqueName { get; set; }

    public string ContentType { get; set; }

    public string UserName { get; set; }

    public string ContentHash { get; set; }

    public virtual FileContentDbModel FileContent { get; set; }
}