using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.Login;
using Application.Login.Authentication;
using Domain.Aggregates;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.Handlers.ClientHandlers;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, LoginResponse>
{
    private readonly IClientRepository _clientRepository;
    private readonly ISalonOwnerRepository _salonOwnerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private ITokenService _tokenService;
    
    public LoginUserHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork,ITokenService tokenService,ISalonOwnerRepository salonOwnerRepository)
    {
        _salonOwnerRepository = salonOwnerRepository;
        _tokenService = tokenService;
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<LoginResponse>> HandleAsyncWithResult(LoginUserCommand command)
    {
        var salonOwnerEmail = "salonowner@gmail.com"; // just for test purpose
        
        // Check if the email belongs to a salon owner
        if (command.email.Equals(salonOwnerEmail))
        {
            return await AuthenticateSalonOwnerAsync(command.email, command.password);
        }
        // Check if the email belongs to a client
        return await AuthenticateClientAsync(command.email, command.password);
    }
    
    private async Task<Result<LoginResponse>> AuthenticateClientAsync(Email email, Password password)
    {
        // Get client by email
        var clientResult = await _clientRepository.GetAsync(email);
        if (!clientResult.IsSuccess)
        {
            return Result<LoginResponse>.Fail(ClientErrorMessage.ClientNotFound());
        }

        var client = clientResult.Data;

        var verifyPasswordResult = clientResult.Data.PasswordValue.Verify(password.ToString());
        if (!verifyPasswordResult)
        {
            return Result<LoginResponse>.Fail(ClientErrorMessage.InvalidCredentials());
        }

        // Generate JWT token with appropriate claims
        var token = _tokenService.GenerateTokenAsync(client.EmailAddress,"Client");

        var response = new LoginResponse(token.Result.Data.Token, token.Result.Data.Role,client.EmailAddress);
        
        await _unitOfWork.SaveChangesAsync();
        return Result<LoginResponse>.Success(response);
    }
    
    private async Task<Result<LoginResponse>> AuthenticateSalonOwnerAsync(Email email, Password password)
    {
        // Get salon owner by email
        var salonOwnerResult = await _salonOwnerRepository.GetAsync(email);
        if (!salonOwnerResult.IsSuccess)
        {
            return Result<LoginResponse>.Fail(SalonOwnerErrorMessage.SalonOwnerNotFound());
        }

        var salonOwner = salonOwnerResult.Data;

        var verifyPasswordResult = salonOwnerResult.Data.PasswordValue.Verify(password.ToString());
        if (!verifyPasswordResult)
        {
            return Result<LoginResponse>.Fail(ClientErrorMessage.InvalidCredentials());
        }

        // Generate JWT token with appropriate claims
        var token = _tokenService.GenerateTokenAsync(salonOwner.EmailAddress,"SalonOwner");

        var response = new LoginResponse(token.Result.Data.Token, token.Result.Data.Role, salonOwner.EmailAddress);
        
        await _unitOfWork.SaveChangesAsync();
        return Result<LoginResponse>.Success(response);
    }
}