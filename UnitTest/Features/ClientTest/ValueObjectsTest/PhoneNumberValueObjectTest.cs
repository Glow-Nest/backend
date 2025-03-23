using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;

namespace UnitTest.Features.ClientTest.ValueObjectsTest;

public class PhoneNumberValueObjectTest
{
    [Fact]
    public void ShouldSucceed_WhenValidProvider()
    {
        // Arrange
        var phoneNumberStr = "91000000";
        
        // Act
        var result = PhoneNumber.Create(phoneNumberStr);
        
        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ShouldFail_WhenEmptyProvided()
    {
        // Arrange
        var phoneNumberStr = "";
        
        // Act
        var result = PhoneNumber.Create(phoneNumberStr);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.PhoneNumberCannotBeEmpty(), result.Errors);
    }

    [Fact]
    public void ShouldFail_WhenInvalidLengthProvided()
    {
        // Arrange
        var phoneNumberStr = "9100000";
        
        // Act
        var result = PhoneNumber.Create(phoneNumberStr);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.PhoneNumberMustBeEightDigitsOnly(), result.Errors);
    }
    
}