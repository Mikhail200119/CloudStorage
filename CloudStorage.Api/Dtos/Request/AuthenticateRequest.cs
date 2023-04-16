namespace CloudStorage.Api.Dtos.Request;

public class AuthenticateRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}