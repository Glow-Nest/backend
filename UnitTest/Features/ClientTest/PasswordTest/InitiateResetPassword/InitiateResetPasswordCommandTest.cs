using Application.AppEntry.Commands.Client;

namespace UnitTest.Features.ClientTest.PasswordTest.InitiateResetPassword;

public class InitiateResetPasswordCommandTest
{
    [Fact]
    public void ShouldReturnSuccess_WhenEmailIsValid()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var result = InitiateResetPasswordCommand.Create(validEmail);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }
    
    [Fact]
    public void ShouldReturnFailure_WhenEmailIsInvalid()
    {
        // Arrange
        var invalidEmail = "invalid-email";

        // Act
        var result = InitiateResetPasswordCommand.Create(invalidEmail);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
    }
}