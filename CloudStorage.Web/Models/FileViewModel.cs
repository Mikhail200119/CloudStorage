namespace CloudStorage.Web.Models;

public class FileViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] Content { get; set; }
    public string UserName { get; set; }
}