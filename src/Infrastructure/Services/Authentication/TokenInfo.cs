using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client.Values;

public class TokenInfo:ValueObject
{
    public string Token { get; }
    public string Role { get; }
    public DateTime ExpiresAt { get; }
    
    public TokenInfo(string token, string role, DateTime expiresAt)
    {
        Token = token;
        Role = role;
        ExpiresAt = expiresAt;
    }
    
    public static Result<TokenInfo> Create(string token, string role, DateTime expiresAt)
    {
        if (string.IsNullOrEmpty(token))
            return Result<TokenInfo>.Fail(ClientErrorMessage.TokenIsEmpty());

        if (string.IsNullOrEmpty(role))
            return Result<TokenInfo>.Fail(ClientErrorMessage.RoleIsEmpty());

        if (expiresAt <= DateTime.UtcNow)
            return Result<TokenInfo>.Fail(ClientErrorMessage.ExpiredDateIsInFuture());

        return Result<TokenInfo>.Success(new TokenInfo(token, role, expiresAt));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Token;
        yield return Role;
        yield return ExpiresAt;
    }
}