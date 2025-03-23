using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client;

public class Client: AggregateRoot<ClientId>
{
    internal FullName fullName;
    internal Email email;
    internal Password password;
    internal PhoneNumber phoneNumber;
    
    protected Client(ClientId clientId,FullName fullName, Email email, Password password, PhoneNumber phoneNumber)
    {
        this.fullName = fullName;
        this.email = email;
        this.password = password;
        this.phoneNumber = phoneNumber;
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