using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client.Entities;

public class OtpSession
{
    internal Email Email { get; }
    public OtpCode OtpCode { get; private set; }
    internal DateTimeOffset CreatedAt { get; }
    internal Purpose Purpose { get; }
    internal bool IsUsed { get; private set; }

    protected OtpSession(Email email, OtpCode otpCode, DateTimeOffset createdAt, Purpose purpose)
    {
        Email = email;
        OtpCode = otpCode;
        CreatedAt = createdAt;
        Purpose = purpose;
    }

    public static Result<OtpSession> Create(Email email, Purpose purpose, IDateTimeProvider dateTimeProvider)
    {
        var otpCode = OtpCode.New().Data;
        var createdAt = dateTimeProvider.GetNow();

        var otpSession = new OtpSession(email, otpCode, createdAt, purpose);
        return Result<OtpSession>.Success(otpSession);
    }

    public Result VerifyOtp(OtpCode otp, Email email, IDateTimeProvider dateTimeProvider)
    {
        if (otp.Value != OtpCode.Value)
        {
            return Result.Fail(ClientErrorMessage.InvalidOtp());
        }

        if (!email.Value.Equals(Email.Value))
        {
            return Result.Fail(ClientErrorMessage.OtpEmailMismatch());
        }

        if (IsUsed)
        {
            return Result.Fail(ClientErrorMessage.OtpAlreadyUsed());
        }

        if (dateTimeProvider.GetNow() > CreatedAt.AddMinutes(2))
        {
            return Result.Fail(ClientErrorMessage.OtpExpired());
        }


        return Result.Success();
    }

    public override string ToString()
    {
        return $"[OtpSession] Code: {OtpCode.Value}, Email: {Email.Value}, Purpose: {Purpose}, CreatedAt: {CreatedAt}";
    }
}