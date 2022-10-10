namespace CloudStorage.BLL.Models;

public class FileUpdateData
{
    public string Name { get; set; }

    public byte[] Content { get; set; }

    public string UserName { get; set; }
}