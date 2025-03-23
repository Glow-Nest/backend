using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;

namespace UnitTest.Features.ClientTest.ValueObjectsTest;

public class FullNameValueObjectTest
{
    [Fact]
    public void ShouldSucceed_WhenValidFullNameProvided()
    {
        // Arrange
        var firstName = "Valid";
        var lastName = "User";

        // Act
        var result = FullName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenInvalidFirstNameProvided()
    {
        // Arrange
        var firstName = "1";
        var lastName = "User";

        // Act
        var result = FullName.Create(firstName, lastName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Errors[0].Message, ClientErrorMessage.InvalidFirstName().Message);
    }
    
    [Fact]
    public void ShouldFail_WhenInvalidLastNameProvided()
    {
        // Arrange
        var firstName = "Valid";
        var lastName = "1";

        // Act
        var result = FullName.Create(firstName, lastName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Errors[0].Message, ClientErrorMessage.InvalidLastName().Message);
    }
}