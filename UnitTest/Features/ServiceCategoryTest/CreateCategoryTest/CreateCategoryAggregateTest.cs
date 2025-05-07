using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.CreateCategoryTest;

public class CreateCategoryAggregateTest
{
    [Fact]
     public async Task ShouldCreate_Category_WithValidInput()
     {
         // Arrange
         var cateogoryName = CategoryName.Create("category1").Data;
         var catDescription = CategoryDescription.Create("description1").Data;
         var mediaUrls = new List<MediaUrl>
         {
             MediaUrl.Create("http://example.com/image1.jpg").Data,
             MediaUrl.Create("http://example.com/image2.jpg").Data
         };
         
         // Act
         var result = await Category.Create(cateogoryName,catDescription, mediaUrls);
         
         // Assert
         Assert.True(result.IsSuccess);
     }
     
     [Fact]
     public void ShouldFail_WhenCategoryNameIsEmpty()
     {
         // Arrange
         var Name = CategoryName.Create("");
         Assert.False(Name.IsSuccess);
     }
     
     [Fact]
     public void ShouldFail_WhenCategoryDescriptionIsEmpty()
     {
         // Arrange
         var Description = CategoryDescription.Create("");
         Assert.False(Description.IsSuccess);
     }

     [Fact]
     public void ShouldFail_WhenMediaUrlIsNull()
     {
         // Arrange
         var mediaUrl = MediaUrl.Create("");
         Assert.False(mediaUrl.IsSuccess);
     }
}