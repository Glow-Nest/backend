using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client;

public class ClientErrorMessage
{
    public static Error ClientNotFound() => new("Client.NotFound", "Client not found.");
    
    // Email
    public static Error InvalidEmailFormat() => new("Email.Invalid", "Email must be in the format: name@example.com");
    public static Error EmailAlreadyExists() => new("Email.AlreadyExists", "Email already exists.");
    
    // FullName
    public static Error InvalidFirstName() => new("FirstName.Length", "Must be 2-25 letters with no symbols or spaces.");
    public static Error InvalidLastName() => new("LastName.Length", "Must be 2-25 letters with no symbols or spaces.");
    
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
    
    // OtpSession
    public static Error InvalidOtp() => new("Otp.Invalid", "Invalid OTP code.");
    public static Error OtpExpired() => new("Otp.Expired", "OTP code has expired. Request new one. ");
    public static Error ActiveOtpAlreadyExists() => new("Otp.Active", "Active OTP code already exists.");
    public static Error NoActiveOtp() => new("Otp.NoActive", "No active OTP code found.");
    public static Error OtpPurposeMismatch() => new("Otp.Purpose", "OTP code purpose does not match.");
    public static Error OtpEmailMismatch() => new("Otp.Email", "OTP code email does not match.");
    public static Error OtpAlreadyUsed() => new("Otp.Used", "OTP code has already been used.");
    public static Error ClientAlreadyVerified() => new("Client.Verified", "Client is already verified.");
    
}