namespace CloudStorage.BLL.Models;

public class File
{
    public int Id { get; set; }

    public string Name { get; set; }

    public byte[] Content { get; set; }

    public string UserName { get; set; }
}