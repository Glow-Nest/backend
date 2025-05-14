using Domain.Aggregates.Client.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Client.UpdateClient;

public class UpdateFullNameCommand : UpdateClientCommandBase
{
    internal FullName FullName { get; }
    public UpdateFullNameCommand(ClientId clientId, FullName fullName) : base(clientId)
    {
        FullName = fullName;
    }
    
    public static Result<UpdateFullNameCommand> Create(string id, string firstname, string lastname)
    {
        var idResult = ClientId.FromGuid(Guid.Parse(id));
        var fullNameResult = FullName.Create(firstname, lastname);
        if (!fullNameResult.IsSuccess)
        {
            return Result<UpdateFullNameCommand>.Fail(fullNameResult.Errors);
        }
        
        return Result<UpdateFullNameCommand>.Success(new UpdateFullNameCommand(idResult, fullNameResult.Data));
    }
}