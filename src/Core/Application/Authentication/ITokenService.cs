using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Application.Login.Authentication;

public interface ITokenService
{
    Task<Result<TokenInfo>> GenerateTokenAsync(string email,string role);
}