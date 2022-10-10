using System.ComponentModel.DataAnnotations.Schema;

namespace CloudStorage.DAL.Entities;

[Table("User")]
public class UserDbModel : IEntity
{
    [Column("UserId")]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
}