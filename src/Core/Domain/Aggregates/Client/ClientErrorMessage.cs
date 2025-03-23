using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client;

public class ClientErrorMessage
{
    // Email
    public static Error InvalidEmailFormat() => new Error("Email.Invalid", "Invalid email format.");
    public static Error EmailAlreadyExists() => new Error("Email.AlreadyExists", "Email already exists.");
    
    // FullName
    public static Error InvalidFirstName() => new Error("FirstName.Length", "Invalid first name. Must be 2-25 letters with no symbols or spaces.");
    public static Error InvalidLastName() => new Error("LastName.Length", "Invalid last name. Must be 2-25 letters with no symbols or spaces.");
    
    // Password
    public static Error PasswordCannotBeEmpty() => new("Password.Empty", "Password cannot be empty.");
    public static Error PasswordLengthOutOfRange() => new("Password.Length", "Password must be between 8 and 25 characters.");
    public static Error PasswordMissingLowercase() => new("Password.Lowercase", "Password must contain at least one lowercase letter.");
    public static Error PasswordMissingUppercase() => new("Password.Uppercase", "Password must contain at least one uppercase letter.");
    public static Error PasswordMissingDigit() => new("Password.Digit", "Password must contain at least one digit.");
    public static Error PasswordMissingSpecialCharacter() => new("Password.Special", "Password must contain at least one special character.");
    
    // PhoneNumber
    public static Error PhoneNumberCannotBeEmpty() => new("Phone.Empty", "Phone number cannot be empty.");
    public static Error PhoneNumberMustBeEightDigitsOnly() => new("Phone.Invalid", "Phone number must be exactly 8 digits, with no spaces or symbols.");
}