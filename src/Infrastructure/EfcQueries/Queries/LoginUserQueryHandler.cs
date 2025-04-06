using Domain.Aggregates.Client;
using Domain.Aggregates.SalonOwner;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using Services.Authentication;

namespace EfcQueries.Queries;

public class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, Result<LoginUserResponse>>
{
    private readonly DomainModelContext _context;
    private readonly ITokenService _tokenService;

    private const string SalonOwnerEmail ="salon@gmail.com";
    
    public LoginUserQueryHandler(DomainModelContext context,ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginUserResponse>> HandleAsync(LoginUserQuery query)
    {
        if (query.Email == SalonOwnerEmail)
        {
            var salonOwner = await _context.Set<SalonOwner>()
                .Where(c => c.EmailAddress == query.Email) 
                .FirstOrDefaultAsync();
            
            if (salonOwner == null || !salonOwner.PasswordValue.Verify(query.Password))
            {
                return Result<LoginUserResponse>.Fail(ClientErrorMessage.InvalidCredentials());
            }
            
            var tokenResult = await _tokenService.GenerateTokenAsync(salonOwner.EmailAddress, "Salon Owner");
            if (!tokenResult.IsSuccess)
            {
                return Result<LoginUserResponse>.Fail(ClientErrorMessage.FailedToGenerateToken());
            }

            var tokenInfo = tokenResult.Data;
            var response = new LoginUserResponse(salonOwner.EmailAddress, "Salon Owner", tokenInfo.Token, tokenInfo.Role);
            return Result<LoginUserResponse>.Success(response);
        }
        
        {
                var client = await _context.Set<Client>()
                    .Where(c => c.Email.Value == query.Email) 
                    .FirstOrDefaultAsync();
                
                if (client == null || !client.Password.Verify(query.Password))
                {
                    return Result<LoginUserResponse>.Fail(ClientErrorMessage.InvalidCredentials());
                }
            
                if (!client.IsVerified)
                {
                    return Result<LoginUserResponse>.Fail(ClientErrorMessage.ClientNotVerified());
                }
                
                var tokenResult = await _tokenService.GenerateTokenAsync(client.Email.Value, "Client");
                if (!tokenResult.IsSuccess)
                {
                    return Result<LoginUserResponse>.Fail(ClientErrorMessage.FailedToGenerateToken());
                }

                var tokenInfo = tokenResult.Data;
                var response = new LoginUserResponse(client.Email.Value, client.FullName.FirstName, tokenInfo.Token, tokenInfo.Role);
                return Result<LoginUserResponse>.Success(response);
        }
        
    }
}