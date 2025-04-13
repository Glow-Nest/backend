using System.Text.RegularExpressions;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client.Values;

public class Password : ValueObject
{
    internal string Value { get; }

    public Password() // for efc
    {
    }

    protected Password(string passwordStr)
    {
        Value = passwordStr;
    }
    
    public static Result<Password> Create(string passwordStr)
    {
        var result = ValidatePassword(passwordStr);
        
        if (!result.IsSuccess)
        {
            return result.ToGeneric<Password>() ;
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordStr);
        var password = new Password(hashedPassword);

        return Result<Password>.Success(password);
    }

    private static Result ValidatePassword(string passwordStr)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(passwordStr))
        {
            errors.Add(ClientErrorMessage.PasswordCannotBeEmpty());
            return Result.Fail(errors); 
        }

        if (passwordStr.Length < 8 || passwordStr.Length > 25)
            errors.Add(ClientErrorMessage.PasswordLengthOutOfRange());

        if (!Regex.IsMatch(passwordStr, @"[a-z]"))
            errors.Add(ClientErrorMessage.PasswordMissingLowercase());

        if (!Regex.IsMatch(passwordStr, @"[A-Z]"))
            errors.Add(ClientErrorMessage.PasswordMissingUppercase());

        if (!Regex.IsMatch(passwordStr, @"\d"))
            errors.Add(ClientErrorMessage.PasswordMissingDigit());

        if (!Regex.IsMatch(passwordStr, @"[^A-Za-z\d]"))
            errors.Add(ClientErrorMessage.PasswordMissingSpecialCharacter());

        return errors.Count > 0 ? Result.Fail(errors) : Result.Success();
    }

    public bool Verify(string passwordToCheck)
    {
        return BCrypt.Net.BCrypt.Verify(passwordToCheck, Value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}