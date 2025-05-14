using OperationResult;

namespace Services.Authentication;

public interface ITokenService
{
    Task<Result<TokenInfo>> GenerateTokenAsync(string email,string role);
}