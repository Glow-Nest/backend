using Domain.Aggregates.Client.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Client;

public class CreateClientCommand(FullName fullName, Email email, Password password, PhoneNumber phoneNumber)
{
    internal FullName fullName = fullName;
    internal Email email = email;
    internal Password password = password;
    internal PhoneNumber phoneNumber = phoneNumber;

    public static  Result<CreateClientCommand> Create(string firstNameStr, string lastNameStr, string emailStr, string passwordStr, string phoneNumberStr)
    {
        var listOfErrors = new List<Error>(); 
         
        var fullNameResult = FullName.Create(firstNameStr, lastNameStr);
        var emailResult = Email.Create(emailStr);
        var passwordResult = Password.Create(passwordStr);
        var phoneNumberResult = PhoneNumber.Create(phoneNumberStr);
        
        if (!fullNameResult.IsSuccess)
        {
             listOfErrors.AddRange(fullNameResult.Errors);
        }
        
        if (!emailResult.IsSuccess)
        {
             listOfErrors.AddRange(emailResult.Errors);
        }
        
        if (!passwordResult.IsSuccess)
        {
             listOfErrors.AddRange(passwordResult.Errors);
        }
        
        if (!phoneNumberResult.IsSuccess)
        {
             listOfErrors.AddRange(phoneNumberResult.Errors);
        }

        if (listOfErrors.Any())
        {
             return Result<CreateClientCommand>.Fail(listOfErrors);
        }
        
        var command = new CreateClientCommand(fullNameResult.Data, emailResult.Data, passwordResult.Data, phoneNumberResult.Data);
        return Result<CreateClientCommand>.Success(command);
    }
}