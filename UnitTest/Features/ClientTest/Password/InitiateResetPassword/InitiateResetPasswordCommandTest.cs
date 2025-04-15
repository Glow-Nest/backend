using Application.AppEntry.Commands.Client;

namespace UnitTest.Features.ClientTest.ResetPassword;

public class InitiateResetPasswordCommandTest
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenEmailIsValid()
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
    public void Create_ShouldReturnFailure_WhenEmailIsInvalid()
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