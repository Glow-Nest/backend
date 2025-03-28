using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;

namespace UnitTest.Features.ClientTest.ValueObjectsTest;

public class PasswordValueObjectTest
{
    [Fact]
    public void ShouldSucceed_WhenValidPasswordProvided()
    {
        // Arrange
        var passwordStr = "Password@123";

        // Act
        var result = Password.Create(passwordStr);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ShouldFail_WhenInvalidLengthPasswordProvider()
    {
        // Arrange
        var passwordStr = "Pass@1";
        
        // Act
        var result = Password.Create(passwordStr);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Errors[0].Message, ClientErrorMessage.PasswordLengthOutOfRange().Message);
    }
    
    [Fact]
    public void ShouldFail_WhenPasswordMissingLowercase()
    {
        // Arrange
        var passwordStr = "PASSWORD@123";
        
        // Act
        var result = Password.Create(passwordStr);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Errors[0].Message, ClientErrorMessage.PasswordMissingLowercase().Message);
    }
    
    [Fact]
    public void ShouldFail_WhenPasswordMissingUppercase()
    {
        // Arrange
        var passwordStr = "password@123";
        
        // Act
        var result = Password.Create(passwordStr);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Errors[0].Message, ClientErrorMessage.PasswordMissingUppercase().Message);
    }
    
    [Fact]
    public void ShouldFail_WhenPasswordMissingDigit()
    {
        // Arrange
        var passwordStr = "Password@";
        
        // Act
        var result = Password.Create(passwordStr);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Errors[0].Message, ClientErrorMessage.PasswordMissingDigit().Message);
    }
    
    [Fact]
    public void ShouldFail_WhenPasswordMissingSpecialCharacter()
    {
        // Arrange
        var passwordStr = "Password123";
        
        // Act
        var result = Password.Create(passwordStr);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Errors[0].Message, ClientErrorMessage.PasswordMissingSpecialCharacter().Message);
    }
}