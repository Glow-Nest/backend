using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Client;

public class ResetPasswordCommand
{
    internal Email Email { get; private set; }
    internal Password NewPassword { get; private set; }
    internal Password ConfirmPassword { get; private set; }

    protected ResetPasswordCommand(Email email, Password newPassword, Password confirmPassword)
    {
        Email = email;
        NewPassword = newPassword;
        ConfirmPassword = confirmPassword;
    }

    public static Result<ResetPasswordCommand> Create(string emailstr,string newPasswordstr, string confirmPasswordstr)
    {
        var listOfErrors = new List<Error>();
        var emailResult = Email.Create(emailstr);
        if (!emailResult.IsSuccess)
        {
            listOfErrors.AddRange(emailResult.Errors);
        }
        var newPasswordResult = Password.Create(newPasswordstr);
        if (!newPasswordResult.IsSuccess)
        {
            listOfErrors.AddRange(newPasswordResult.Errors);
        }
        
        var confirmPasswordResult = Password.Create(confirmPasswordstr);
        if (!confirmPasswordResult.IsSuccess)
        {
            listOfErrors.AddRange(confirmPasswordResult.Errors);
        }
        
        if (newPasswordstr != confirmPasswordstr)
        {
            listOfErrors.Add(ClientErrorMessage.PasswordDoesntMatch());
        }

        if (listOfErrors.Any())
        {
            return Result<ResetPasswordCommand>.Fail(listOfErrors);
        }
        
        var command = new ResetPasswordCommand(emailResult.Data,newPasswordResult.Data, confirmPasswordResult.Data);
        return Result<ResetPasswordCommand>.Success(command);
    }
}