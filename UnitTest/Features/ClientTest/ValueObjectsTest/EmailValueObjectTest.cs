using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;

namespace UnitTest.Features.ClientTest.ValueObjectsTest;

public class EmailValueObjectTest
{
    [Fact]
    public void ShouldSucceed_WhenValidEmailProvided()
    {
        // Arrange
        var emailStr = "rajp@gmail.com";

        // Act
        var result = Email.Create(emailStr);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("rajp@gmail")]
    [InlineData("rajp space@example.com")]
    public void ShouldFail_WhenInvalidEmailProvided(string emailStr)
    {
        // Act
        var result = Email.Create(emailStr);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Errors[0].Message, ClientErrorMessage.InvalidEmailFormat().Message);
    }
}