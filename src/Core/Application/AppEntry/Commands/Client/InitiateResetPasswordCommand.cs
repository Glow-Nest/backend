using Domain.Aggregates.Client.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Client;

public class InitiateResetPasswordCommand
{
    internal Email email;
    
    protected InitiateResetPasswordCommand(Email email)
    {
        this.email = email;
    }
    
    public static Result<InitiateResetPasswordCommand> Create(string emailStr)
    {
        var emailResult = Email.Create(emailStr);
        
        if (!emailResult.IsSuccess)
        {
            return Result<InitiateResetPasswordCommand>.Fail(emailResult.Errors);
        }
        
        var command = new InitiateResetPasswordCommand(emailResult.Data);
        return Result<InitiateResetPasswordCommand>.Success(command);
    }
    
}