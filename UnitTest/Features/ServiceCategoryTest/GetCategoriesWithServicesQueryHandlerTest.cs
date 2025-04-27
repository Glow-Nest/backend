using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;
using EfcQueries.Queries;
using Moq;
using QueryContracts.Queries.Service;

namespace UnitTest.Features.ServiceCategoryTest;

public class GetCategoriesWithServicesQueryHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly GetAllCategoryWithServiceQueryHandler _handler;
    
    public GetCategoriesWithServicesQueryHandlerTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new GetAllCategoryWithServiceQueryHandler(_categoryRepositoryMock.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenCategoriesWithServicesExist()
    {
        // Arrange
        var categories = new List<Category>();

        var categoryName = CategoryName.Create("Beauty").Data;
        var description = CategoryDescription.Create("Beauty treatments").Data;
        var mediaUrls = new List<MediaUrl> { MediaUrl.Create("https://example.com/beauty.jpg").Data };
        var categoryResult = await Category.Create(categoryName, description, mediaUrls);
        var category = categoryResult.Data;

        var serviceName = ServiceName.Create("Facial Treatment").Data;
        var price = Price.Create(200).Data;
        var duration = TimeSpan.FromMinutes(60);

        var serviceResult = await Service.Create(serviceName, price, duration);
        var service = serviceResult.Data;
        category.Services.Add(service);

        categories.Add(category);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoriesWithServicesAsync())
            .ReturnsAsync(Result<List<Category>>.Success(categories));

        var query = new GetAllCategoriesWithServices.Query();

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data.Categories);
        var categoryDto = result.Data.Categories.First();
        Assert.Equal("Beauty", categoryDto.Name);
        Assert.Single(categoryDto.Services);
        var serviceDto = categoryDto.Services.First();
        Assert.Equal("Facial Treatment", serviceDto.Name);
        Assert.Equal(200, serviceDto.Price);
        Assert.Equal("01:00:00", serviceDto.Duration); // TimeSpan.ToString() gives this format
    }
}