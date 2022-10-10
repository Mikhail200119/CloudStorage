using CloudStorage.BLL.Models;

namespace CloudStorage.BLL.Services;

public interface IUserService
{
    User Current { get; }
}