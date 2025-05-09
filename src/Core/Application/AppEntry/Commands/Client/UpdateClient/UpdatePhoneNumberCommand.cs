using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Client.UpdateClient;

public class UpdatePhoneNumberCommand : UpdateClientCommandBase
{
    internal PhoneNumber PhoneNumber { get; }
    
    protected UpdatePhoneNumberCommand(ClientId clientId, PhoneNumber phoneNumber) : base(clientId)
    {
        PhoneNumber = phoneNumber;
    }
    
    public static Result<UpdatePhoneNumberCommand> Create(string id, string phoneNumber)
    {
        var idResult = ClientId.FromGuid(Guid.Parse(id));
        var phoneNumberResult = PhoneNumber.Create(phoneNumber);
        if (!phoneNumberResult.IsSuccess)
        {
            return Result<UpdatePhoneNumberCommand>.Fail(phoneNumberResult.Errors);
        }
        
        return Result<UpdatePhoneNumberCommand>.Success(new UpdatePhoneNumberCommand(idResult, phoneNumberResult.Data));
    }
}