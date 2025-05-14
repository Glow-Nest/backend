using System.Text.RegularExpressions;
using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Client.Values;

public class PhoneNumber : ValueObject
{
    internal string Value { get; }

    protected PhoneNumber(string value)
    {
        Value = value;
    }
    
    public static Result<PhoneNumber> Create(string phoneNumberStr)
    {
        
        var result = ValidatePhoneNumber(phoneNumberStr);

        if (!result.IsSuccess)
        {
            return result.ToGeneric<PhoneNumber>();
        }

        var phoneNumber = new PhoneNumber(phoneNumberStr);
        return Result<PhoneNumber>.Success(phoneNumber);
    }
    
    private static Result ValidatePhoneNumber(string phoneNumber)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            errors.Add(ClientErrorMessage.PhoneNumberCannotBeEmpty());
            return Result.Fail(errors);
        }

        if (!Regex.IsMatch(phoneNumber, @"^\d{8}$"))
        {
            errors.Add(ClientErrorMessage.PhoneNumberMustBeEightDigitsOnly());
        }

        return errors.Count > 0 ? Result.Fail(errors) : Result.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}