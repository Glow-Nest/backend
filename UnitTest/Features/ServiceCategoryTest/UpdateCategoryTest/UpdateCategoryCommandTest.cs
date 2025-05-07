using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateCategoryTest;

public class UpdateCategoryCommandTest
{
    [Fact]
    public void ShouldReturnSuccess_WhenCategoryIsUpdated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var categoryName = CategoryName.Create("category1").Data;
        var descriptionstr = CategoryDescription.Create("this is cat").Data;
        var mediaUrls = new List<string>
        {
            "https://example.com/image1.jpg",
            "https://example.com/image2.jpg"
        };
        
        //Act
        var command = UpdateCategoryCommand.Create(categoryId.Value.ToString() ,categoryName.Value, descriptionstr.Value, mediaUrls);

        // Assert
        Assert.True(command.IsSuccess);
    }
}