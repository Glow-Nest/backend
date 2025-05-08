using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateMediaUrlTest;

public class UpdateMediaUrlCommandTest
{
    [Fact]
    public void ShouldReturnSuccess_WhenCategoryMediaURlIsUpdated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var mediaUrl = new List<MediaUrl>
        {
            MediaUrl.Create("http://example.com/image1.jpg").Data,
            MediaUrl.Create("http://example.com/image2.jpg").Data
        };
        
        //Act
        var command = UpdateMediaUrlCommand.Create(categoryId.Value.ToString() ,mediaUrl.Select(x => x.Value).ToList());

        // Assert
        Assert.True(command.IsSuccess);
    }
}