using System.Security.Cryptography;
using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Client.Values;

public class OtpCode : ValueObject
{
    public string Value { get; private set; }
    
    protected OtpCode(string value)
    {
        Value = value;
    }

    public static Result<OtpCode> New()
    {
        var code = GenerateSecureOtp(4);
        var otpCode = new OtpCode(code);
        return Result<OtpCode>.Success(otpCode);
    }
    
    public static Result<OtpCode> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 4)
        {
            return Result<OtpCode>.Fail(ClientErrorMessage.InvalidOtp());
        }

        var otpCode = new OtpCode(value);
        return Result<OtpCode>.Success(otpCode);
    }
    
    private static string GenerateSecureOtp(int length)
    {
        const string digits = "0123456789";
        var bytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = digits[bytes[i] % digits.Length];
        }

        return new string(chars);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}