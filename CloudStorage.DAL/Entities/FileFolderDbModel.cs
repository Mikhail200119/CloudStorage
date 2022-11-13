using System.ComponentModel.DataAnnotations.Schema;
using CloudStorage.DAL.Entities.Interfaces;

namespace CloudStorage.DAL.Entities;

[Table("FileFolder")]
public class FileFolderDbModel : IEntity
{
    [Column("FileFolderId")]
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentFolderId { get; set; }
    public FileFolderDbModel ParentFolder { get; set; }
}