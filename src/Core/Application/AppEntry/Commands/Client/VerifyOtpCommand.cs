using Domain.Aggregates.Client.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Client;

public class VerifyOtpCommand
{
    internal Email email;
    internal OtpCode otpCode;

    protected VerifyOtpCommand(Email email, OtpCode otpCode)
    {
        this.email = email;
        this.otpCode = otpCode;
    }

    public static Result<VerifyOtpCommand> Create(string emailStr, string otpCodeStr)
    {
        var listOfErrors = new List<Error>();
        
        var emailResult = Email.Create(emailStr);
        var otpCodeResult = OtpCode.Create(otpCodeStr);
        
        if (!emailResult.IsSuccess)
        {
            listOfErrors.AddRange(emailResult.Errors);
        }
        
        if (!otpCodeResult.IsSuccess)
        {
            listOfErrors.AddRange(otpCodeResult.Errors);
        }
        
        if (listOfErrors.Any())
        {
            return Result<VerifyOtpCommand>.Fail(listOfErrors);
        }
        
        var command = new VerifyOtpCommand(emailResult.Data, otpCodeResult.Data);
        return Result<VerifyOtpCommand>.Success(command);
    }
}