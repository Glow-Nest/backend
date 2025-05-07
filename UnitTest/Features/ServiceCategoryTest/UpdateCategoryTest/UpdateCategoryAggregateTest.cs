using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateCategoryTest;

public class UpdateCategoryAggregateTest
{
    [Fact]
    public async Task ShouldUpdate_Category_WithValidInput()
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
        var updateCatDescription = CategoryDescription.Create("description7").Data;
        var updateMediaUrls = new List<MediaUrl>
        {
            MediaUrl.Create("http://example.com/image3.jpg").Data,
            MediaUrl.Create("http://example.com/image4.jpg").Data
        };
        
        // Act
        var result = category.UpdateCategory(updateCateogoryName, updateCatDescription, updateMediaUrls);
         
        // Assert
        Assert.True(result.IsSuccess);
    }
}