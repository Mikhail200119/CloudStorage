using System.Security.Claims;
using System.Text.RegularExpressions;
using CloudStorage.BLL.Models;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.BLL.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public User Current => BuildUserModel(_httpContextAccessor.HttpContext.User.Claims);

    private static User BuildUserModel(IEnumerable<Claim> claims) =>
        new()
        {
            Email = claims.FirstOrDefault(claim=>claim.Type.EndsWith("emailaddress"))?.Value ?? throw new ApplicationException()
        };
}