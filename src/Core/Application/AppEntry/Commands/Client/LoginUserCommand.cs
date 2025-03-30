using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Client;

public class LoginUserCommand
{
    internal Email email;
    internal Password password;
    
    public LoginUserCommand(Email email, Password password)
    {
        this.email = email;
        this.password = password;
    }
    
    public static  Result<LoginUserCommand> Create(string emailStr, string passwordStr)
    {
        var listOfErrors = new List<Error>(); 
         
        var emailResult = Email.Create(emailStr);
        var passwordResult = Password.Create(passwordStr);
        
        if (!emailResult.IsSuccess)
        {
            listOfErrors.AddRange(emailResult.Errors);
        }
        
        if (!passwordResult.IsSuccess)
        {
            listOfErrors.AddRange(passwordResult.Errors);
        }

        if (listOfErrors.Any())
        {
            return Result<LoginUserCommand>.Fail(listOfErrors);
        }
        
        var command = new LoginUserCommand(emailResult.Data, passwordResult.Data);
        return Result<LoginUserCommand>.Success(command);
    }
}