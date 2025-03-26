using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Entities;
using Domain.Aggregates.Client.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client;

public class Client : AggregateRoot<ClientId>
{
    internal ClientId ClientId { get; private set; }
    internal FullName FullName { get; private set; }
    internal Email Email { get; private set; }
    internal Password Password { get; private set; }
    internal PhoneNumber PhoneNumber { get; private set; }
    internal OtpSession? OtpSession { get; private set; }
    internal bool IsVerified { get; private set; }

    public string EmailAddress => Email.Value;
    public Email EmailValue => Email;

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
        if (OtpSession is not null && dateTimeProvider.GetNow() > OtpSession.CreatedAt.AddMinutes(10))
        {
            return Result<OtpSession>.Fail(ClientErrorMessage.ActiveOtpAlreadyExists());
        }

        var otpSessionResult = OtpSession.Create(Email, purpose, dateTimeProvider);

        if (!otpSessionResult.IsSuccess)
        {
            return otpSessionResult;
        }

        OtpSession = otpSessionResult.Data;
        return Result<OtpSession>.Success(otpSessionResult.Data);
    }

    public Result VerifyOtp(OtpCode otp, IDateTimeProvider dateTimeProvider)
    {
        if (OtpSession is null)
        {
            return Result.Fail(ClientErrorMessage.NoActiveOtp());
        }

        var verifyResult = OtpSession.VerifyOtp(otp, Email, dateTimeProvider);

        if (!verifyResult.IsSuccess)
        {
            return verifyResult;
        }

        IsVerified = true;
        return Result.Success();
    }
}