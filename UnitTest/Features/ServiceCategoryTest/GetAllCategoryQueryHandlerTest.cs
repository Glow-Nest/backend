using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;
using EfcQueries.Queries;
using EfcQueries.Queries.Category;
using Moq;
using OperationResult;
using QueryContracts.Queries.Service;

namespace UnitTest.Features.ServiceCategoryTest;

public class GetAllCategoryQueryHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly GetAllCategoryQueryHandler _handler;
    
    public GetAllCategoryQueryHandlerTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new GetAllCategoryQueryHandler(_categoryRepositoryMock.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenCategoriesExist()
    {
        // Arrange
        var categories = new List<Category>();

        var categoryName = CategoryName.Create("Test Category").Data;
        var description = CategoryDescription.Create("Test Description").Data;
        var mediaUrls = new List<MediaUrl> { MediaUrl.Create("https://example.com/image.jpg").Data };
        var categoryResult = await Category.Create(categoryName, description, mediaUrls);
            
        categories.Add(categoryResult.Data);

        _categoryRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(Result<List<Category>>.Success(categories));

        var query = new GetAllCategory.Query();

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data.Categories);
        Assert.Equal("Test Category", result.Data.Categories.First().Name);
        Assert.Equal("Test Description", result.Data.Categories.First().Description);
        Assert.Contains("https://example.com/image.jpg", result.Data.Categories.First().MediaUrls);
    }
    
    
}