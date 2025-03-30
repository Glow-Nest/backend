namespace Application.Login;

public class LoginResponse
{
    private string Token { get; }
    private string Role{ get; }
    private string Email{ get; }
    
    public LoginResponse(string token, string role, string email)
    {
        Token = token;
        Role = role;
        Email = email;
    }
}