using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateCategoryNameTest;

public class UpdateCategoryNameCommandTest
{
    [Fact]
    public void ShouldReturnSuccess_WhenCategoryNameIsUpdated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var categoryName = CategoryName.Create("category1").Data;
        
        //Act
        var command = UpdateCategoryNameCommand.Create(categoryId.Value.ToString() ,categoryName.Value);

        // Assert
        Assert.True(command.IsSuccess);
    }
}