using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateMediaUrlTest;

public class UpdateMediaUrlAggregateTest
{
    [Fact]
    public async Task ShouldUpdate_CategoryMediaUrl_WithValidInput()
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
        
        var updateMediaUrl = new List<MediaUrl>
        {
            MediaUrl.Create("http://example.com/image146.jpg").Data,
            MediaUrl.Create("http://example.com/imagew452.jpg").Data
        };
        
        // Act
        var result = category.UpdateCategoryMediaUrls(updateMediaUrl);
         
        // Assert
        Assert.True(result.IsSuccess);
    }
}