namespace CloudStorage.BLL.Models;

public class FileCreateData
{
    public string Name { get; set; }

    public byte[] Content { get; set; }

    public string ContentType { get; set; }
}