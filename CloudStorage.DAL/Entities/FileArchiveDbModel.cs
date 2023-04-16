using System.ComponentModel.DataAnnotations.Schema;
using CloudStorage.DAL.Entities.Interfaces;

namespace CloudStorage.DAL.Entities;

[Table("FileArchive")]
public class FileArchiveDbModel : IEntity
{
    [Column("FileArchiveId")]
    public int Id { get; set; }
    public string ProvidedName { get; set; }
    public string UniqueName { get; set; }
    public string Extension { get; set; }
    public IEnumerable<FileDescriptionDbModel> Files { get; set; }
}