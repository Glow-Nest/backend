using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Client.UpdateClient;

public class UpdatePasswordCommand : UpdateClientCommandBase
{
    internal Password Password { get; }
    
    protected UpdatePasswordCommand(ClientId clientId,Password password) : base(clientId)
    {
        Password = password;
    }
    
    public static Result<UpdatePasswordCommand> Create(string id, string password)
    {
        var idResult = ClientId.FromGuid(Guid.Parse(id));
        var passwordResult = Password.Create(password);
        if (!passwordResult.IsSuccess)
        {
            return Result<UpdatePasswordCommand>.Fail(passwordResult.Errors);
        }
        
        return Result<UpdatePasswordCommand>.Success(new UpdatePasswordCommand(idResult, passwordResult.Data));
    }
}