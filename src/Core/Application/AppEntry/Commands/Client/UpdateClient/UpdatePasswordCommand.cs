using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Client.UpdateClient;

public class UpdatePasswordCommand : UpdateClientCommandBase
{
    internal Password NewPassword { get; }
    internal Password ConfirmPassword { get; }
    
    protected UpdatePasswordCommand(ClientId clientId,Password newPassword,Password confirmPassword) : base(clientId)
    {
        NewPassword = newPassword;
        ConfirmPassword = confirmPassword;
    }
    
    public static Result<UpdatePasswordCommand> Create(string id, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            return Result<UpdatePasswordCommand>.Fail(ClientErrorMessage.PasswordDoesntMatch());
        }

        var idResult = ClientId.FromGuid(Guid.Parse(id));
        var passwordResult = Password.Create(newPassword); 

        if (!passwordResult.IsSuccess)
        {
            return Result<UpdatePasswordCommand>.Fail(passwordResult.Errors);
        }

        return Result<UpdatePasswordCommand>.Success(
            new UpdatePasswordCommand(idResult, passwordResult.Data, passwordResult.Data) 
        );
    }

}