using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateCategoryNameTest;

public class UpdateCategoryNameAggregateTest
{
    [Fact]
    public async Task ShouldUpdate_CategoryName_WithValidInput()
    {
        // Arrange
        var cateogoryName = CategoryName.Create("category1").Data;
        var catDescription = CategoryDescription.Create("description1").Data;
        var mediaUrls = new List<MediaUrl>
        {
            MediaUrl.Create("http://example.com/image1.jpg").Data,
            MediaUrl.Create("http://example.com/image2.jpg").Data
        };
         
        var categoryResult = await Category.Create(cateogoryName,catDescription, mediaUrls);
        var category = categoryResult.Data;
        
        var updateCateogoryName = CategoryName.Create("category1").Data;
        
        // Act
        var result = category.UpdateCategoryName(updateCateogoryName);
         
        // Assert
        Assert.True(result.IsSuccess);
    }
}