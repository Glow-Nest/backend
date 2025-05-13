using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using OperationResult;

namespace UnitTest.Features.Helpers.Builders;

public class ClientBuilder
{
    private string _email = "validU@gmail.com";
    private string _firstName = "Valid";
    private string _lastName = "User";
    private string _password = "Password@123";
    private string _phoneNumber = "91000000";
    
    private static IClientRepository _clientRepository = new FakeClientRepository();
    private IEmailUniqueChecker _emailUniqueChecker = new FakeEmailUniqueChecker(_clientRepository);

    private ClientBuilder()
    {
    }

    public static ClientBuilder CreateValid() => new();

    public async Task<Result<Client>> BuildAsync()
    {
        var fullNameResult = FullName.Create(_firstName, _lastName);
        var emailResult = Email.Create(_email);
        var passwordResult = Password.Create(_password);
        var phoneNumberResult = PhoneNumber.Create(_phoneNumber);

        if (!fullNameResult.IsSuccess)
        {
            return Result<Client>.Fail(fullNameResult.Errors);
        }

        if (!emailResult.IsSuccess)
        {
            return Result<Client>.Fail(emailResult.Errors);
        }

        if (!passwordResult.IsSuccess)
        {
            return Result<Client>.Fail(passwordResult.Errors);
        }

        if (!phoneNumberResult.IsSuccess)
        {
            return Result<Client>.Fail(phoneNumberResult.Errors);
        }

        var result = await Client.Create(fullNameResult.Data, emailResult.Data, passwordResult.Data, phoneNumberResult.Data, _emailUniqueChecker);
        
        if (!result.IsSuccess)
        {
            return Result<Client>.Fail(result.Errors);
        }

        return Result<Client>.Success(result.Data);
    }
}