using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.DomainEvents;
using Domain.Aggregates.Client.Entities;
using Domain.Aggregates.Client.Values;
using Domain.Common;
using Domain.Common.BaseClasses;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client;

public class Client : AggregateRoot
{
    public ClientId ClientId { get; private set; }
    public FullName FullName { get; private set; }
    internal Email Email { get; private set; }
    internal Password Password { get; private set; }
    internal PhoneNumber PhoneNumber { get; private set; }
    internal OtpSession? OtpSession { get; private set; }
    internal bool IsVerified { get; set; }

    public string EmailAddress => Email.Value;
    public Password PasswordValue => Password;

    public Client() // for EF Core
    {
    }

    protected Client(ClientId clientId, FullName fullName, Email email, Password password, PhoneNumber phoneNumber)
    {
        ClientId = clientId;
        FullName = fullName;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        IsVerified = false;
    }

    public static async Task<Result<Client>> Create(FullName fullName, Email email, Password password,
        PhoneNumber phoneNumber, IEmailUniqueChecker emailUniqueChecker)
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

    public Result<OtpSession> CreateOtp(Purpose purpose, IDateTimeProvider dateTimeProvider)
    {
        if (OtpSession is not null && dateTimeProvider.GetNow() > OtpSession.CreatedAt.AddMinutes(2))
        {
            return Result<OtpSession>.Fail(ClientErrorMessage.ActiveOtpAlreadyExists());
        }
        
        if (IsVerified && purpose == Purpose.Registration)
        {
            return Result<OtpSession>.Fail(ClientErrorMessage.ClientAlreadyVerified());
        }

        var otpSessionResult = OtpSession.Create(Email, purpose, dateTimeProvider);

        if (!otpSessionResult.IsSuccess)
        {
            return otpSessionResult;
        }

        OtpSession = otpSessionResult.Data;
        
        AddDomainEvent(new OtpCreatedDomainEvent(OtpSession.OtpCode, this));
        
        return Result<OtpSession>.Success(otpSessionResult.Data);
    }

    public Result VerifyOtp(OtpCode otp, IDateTimeProvider dateTimeProvider)
    {
        if (OtpSession is null)
        {
            return Result.Fail(ClientErrorMessage.NoActiveOtp());
        }

        if (IsVerified && OtpSession.Purpose == Purpose.Registration)
        {
            return Result.Fail(ClientErrorMessage.ClientAlreadyVerified());
        }

        var verifyResult = OtpSession.VerifyOtp(otp, Email, dateTimeProvider);

        if (!verifyResult.IsSuccess)
        {
            return verifyResult;
        }

        IsVerified = true;
        OtpSession.IsUsed = true;
        return Result.Success();
    }

    public Result<OtpSession> CreatePasswordResetOtp(IDateTimeProvider dateTimeProvider)
    {
        return CreateOtp(Purpose.PasswordReset, dateTimeProvider);
    }

    public Result ResetPassword(Password newPasswordStr)
    {
        if (OtpSession is not { Purpose: Purpose.PasswordReset })
        {
            return Result.Fail(ClientErrorMessage.InvalidOtpVerification());
        }

        if (!OtpSession.IsUsed)
        {
            return Result.Fail(ClientErrorMessage.ClientNotVerified());
        }
        
        Password = newPasswordStr;
        OtpSession = null;
        
        return Result.Success();
    }
}