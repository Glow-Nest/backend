using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.CreateCategoryTest;

public class CreateCategoryCommandTest
{
    [Fact]
    public void CreateServiceCommand_ShouldReturnSuccess_WhenServiceIsCreated()
    {
        // Arrange
        var categoryName = CategoryName.Create("category1").Data;
        var descriptionstr = CategoryDescription.Create("this is cat").Data;
        var mediaUrls = new List<string>
        {
            "https://example.com/image1.jpg",
            "https://example.com/image2.jpg"
        };
        
        //Act
        var command = CreateCategoryCommand.Create(categoryName.Value, descriptionstr.Value, mediaUrls);

        // Assert
        Assert.True(command.IsSuccess);
    }
}