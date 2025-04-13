using Domain.Aggregates.Client;
using Domain.Aggregates.SalonOwner;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using Services.Authentication;

namespace EfcQueries.Queries;

public class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, Result<LoginUserResponse>>
{
    private readonly DomainModelContext _context;
    private readonly ITokenService _tokenService;
    private readonly string _salonOwnerEmail;
    
    public LoginUserQueryHandler(DomainModelContext context, ITokenService tokenService, IConfiguration? configuration)
    {
        _context = context;
        _tokenService = tokenService; 
        _salonOwnerEmail = configuration?.GetSection("SalonOwner:Email").Value ?? string.Empty;
    }

    public async Task<Result<LoginUserResponse>> HandleAsync(LoginUserQuery query)
    {
        if (string.Equals(query.Email?.Trim(), _salonOwnerEmail, StringComparison.OrdinalIgnoreCase))

        {
            var salonOwner = await _context.Set<SalonOwner>()
                .Where(c => c.Email.Value == query.Email) 
                .FirstOrDefaultAsync();
            
            if (salonOwner == null)
            {
                return Result<LoginUserResponse>.Fail(SalonOwnerErrorMessage.SalonOwnerNotFound());
            }

            // Password verification
            if (!salonOwner.Password.Verify(query.Password))
            {
                return Result<LoginUserResponse>.Fail(ClientErrorMessage.InvalidCredentials());
            }
            
            var tokenResult = await _tokenService.GenerateTokenAsync(salonOwner.Email.Value, "Salon Owner");
            if (!tokenResult.IsSuccess)
            {
                return Result<LoginUserResponse>.Fail(ClientErrorMessage.FailedToGenerateToken());
            }

            var tokenInfo = tokenResult.Data;
            var response = new LoginUserResponse(salonOwner.Email.Value, "Salon Owner", tokenInfo.Token, tokenInfo.Role);
            return Result<LoginUserResponse>.Success(response);
        }

        {
            var client = await _context.Set<Client>()
                .FirstOrDefaultAsync(c => c.Email.Value == query.Email);
                
            if (client == null)
            {
                return Result<LoginUserResponse>.Fail(ClientErrorMessage.ClientNotFound());
            }
            
            // Password verification
            if (!client.Password.Verify(query.Password))
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