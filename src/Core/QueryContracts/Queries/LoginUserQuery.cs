using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

public record LoginUserResponse(Guid id, string Email, string FirstName, string LastName, string PhoneNumber, string Token, string Role);
public class LoginUserQuery: IQuery<Result<LoginUserResponse>>
{
    public string Email { get; private set; }
    public string Password { get; private set; }
    
    public LoginUserQuery(string email, string password)
    {
        Email = email;
        Password = password;
    }
}