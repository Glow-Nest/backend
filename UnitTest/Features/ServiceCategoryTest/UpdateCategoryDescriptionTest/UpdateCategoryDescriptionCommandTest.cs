using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateCategoryDescriptionTest;

public class UpdateCategoryDescriptionCommandTest
{
    [Fact]
    public void ShouldReturnSuccess_WhenCategoryDescriptionIsUpdated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var categoryDescription = CategoryDescription.Create("category1").Data;
        
        //Act
        var command = UpdateCategoryDescriptionCommand.Create(categoryId.Value.ToString() ,categoryDescription.Value);

        // Assert
        Assert.True(command.IsSuccess);
    }
}