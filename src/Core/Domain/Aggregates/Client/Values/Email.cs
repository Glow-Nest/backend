using System.Text.RegularExpressions;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client.Values;

public class Email : ValueObject
{
    public string Value { get; private set; }

    protected Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string emailStr)
    {
        var result = ValidateEmail(emailStr);

        if (!result.IsSuccess)
        {
            return result.ToGeneric<Email>();
        }

        var email = new Email(emailStr);
        return Result<Email>.Success(email);
    }

    private static Result ValidateEmail(string email)
    {
        const string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        var isMatch = Regex.IsMatch(email, emailPattern);

        if (!isMatch)
        {
            return Result.Fail(ClientErrorMessage.InvalidEmailFormat());
        }

        return Result.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}