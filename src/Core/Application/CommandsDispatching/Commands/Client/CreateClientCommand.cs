using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Application.CommandsDispatching.Commands.Client;

public class CreateClientCommand(FullName fullName, Email email, Password password, PhoneNumber phoneNumber)
{
    internal FullName fullName = fullName;
    internal Email email = email;
    internal Password password = password;
    internal PhoneNumber phoneNumber = phoneNumber;

    public static  Result<CreateClientCommand> Create(string firstNameStr, string lastNameStr, string emailStr, string passwordStr, string phoneNumberStr)
    {
        var fullNameResult = FullName.Create(firstNameStr, lastNameStr);
        var emailResult = Email.Create(emailStr);
        var passwordResult = Password.Create(passwordStr);
        var phoneNumberResult = PhoneNumber.Create(phoneNumberStr);
        
        if (!fullNameResult.IsSuccess)
        {
            return Result<CreateClientCommand>.Fail(fullNameResult.Errors);
        }
        
        if (!emailResult.IsSuccess)
        {
            return Result<CreateClientCommand>.Fail(emailResult.Errors);
        }
        
        if (!passwordResult.IsSuccess)
        {
            return Result<CreateClientCommand>.Fail(passwordResult.Errors);
        }
        
        if (!phoneNumberResult.IsSuccess)
        {
            return Result<CreateClientCommand>.Fail(phoneNumberResult.Errors);
        }
        
        var command = new CreateClientCommand(fullNameResult.Data, emailResult.Data, passwordResult.Data, phoneNumberResult.Data);
        return Result<CreateClientCommand>.Success(command);
    }
}