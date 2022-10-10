using System.ComponentModel.DataAnnotations.Schema;

namespace CloudStorage.DAL.Entities;

[Table("File")]
public class FileDbModel : IEntity
{
    [Column("FileId")]
    public int Id { get; set; }

    public string ProvidedName { get; set; }

    public string UniqueName { get; set; }

    public byte[] Content { get; set; }

    public string UserName { get; set; }
}