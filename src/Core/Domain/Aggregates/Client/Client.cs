using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client;

public class Client: AggregateRoot<ClientId>
{
    internal ClientId ClientId { get; private set; }
    internal FullName FullName {get; private set;}
    internal Email Email { get; private set;}
    internal Password Password { get; private set;}
    internal PhoneNumber PhoneNumber { get; private set;}
    
    public string EmailAddress => Email.Value;
    
    protected Client(ClientId clientId, FullName fullName, Email email, Password password, PhoneNumber phoneNumber)
    {
        ClientId = clientId;
        FullName = fullName;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
    }

    public static async Task<Result<Client>> Create(FullName fullName, Email email, Password password, PhoneNumber phoneNumber, IEmailUniqueChecker emailUniqueChecker)
    {
        var clientId = ClientId.Create();
        var emailUnique = await emailUniqueChecker.IsEmailUniqueAsync(email.Value);

        if (!emailUnique)
        {
            return Result<Client>.Fail(ClientErrorMessage.EmailAlreadyExists());
        }

        var client = new Client(clientId, fullName, email, password, phoneNumber);
        return Result<Client>.Success(client);
    }
}