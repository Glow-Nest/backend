using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Client;

public class CreateOtpCommand
{
    internal Email Email { get; private set; }
    internal Purpose Purpose { get; private set; }

    protected CreateOtpCommand(Email email, Purpose purpose)
    {
        Email = email;
        Purpose = purpose;
    }

    public static Result<CreateOtpCommand> Create(string email, string purpose)
    {
        var listOfErrors = new List<Error>();

        var emailResult = Email.Create(email);
        var purposeResult = purpose == "Registration" ? Purpose.Registration : Purpose.PasswordReset;

        if (!emailResult.IsSuccess)
        {
            listOfErrors.AddRange(emailResult.Errors);
        }

        if (purposeResult == Purpose.PasswordReset)
        {
            listOfErrors.Add(ClientErrorMessage.OtpPurposeMismatch());
        }

        if (listOfErrors.Any())
        {
            return Result<CreateOtpCommand>.Fail(listOfErrors);
        }

        var otpCommand = new CreateOtpCommand(emailResult.Data, purposeResult);
        return Result<CreateOtpCommand>.Success(otpCommand);
    }
}