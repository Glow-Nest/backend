using Application.AppEntry.Commands.ServiceCategory;
using Application.Handlers.ServiceCategoryHandlers;
using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;
using Moq;

namespace UnitTest.Features.ServiceCategoryTest.CreateServiceTest;

public class CreateServiceHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly AddServiceInCategoryHandler _handler;
    
    public CreateServiceHandlerTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new AddServiceInCategoryHandler(_categoryRepositoryMock.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenServiceIsCreated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        string serviceDuration = "1";
        
        var command = AddServiceInCategoryCommand.Create(serviceName.Value, servicePrice.Value, serviceDuration, categoryId.Value.ToString()).Data;
        
        // Create a real Category object
        var categoryName = CategoryName.Create("Test Category").Data;
        var categoryDescription = CategoryDescription.Create("Test Description").Data;
        var mediaUrls = new List<MediaUrl>();

        var categoryCreateResult = await Category.Create(categoryName, categoryDescription, mediaUrls);
        var category = categoryCreateResult.Data;

        _categoryRepositoryMock
            .Setup(repo => repo.GetAsync(command.categoryId))
            .ReturnsAsync(Result<Category>.Success(category));
        
        // Act
        var result = await _handler.HandleAsync(command);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}