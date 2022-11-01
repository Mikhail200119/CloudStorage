namespace CloudStorage.BLL.Services.Interfaces;

public interface IDataHasher
{
    string HashData(byte[] data);
}