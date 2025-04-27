using Application.AppEntry.Commands.ServiceCategory;
using Application.Handlers.ServiceCategoryHandlers;
using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;
using Moq;

namespace UnitTest.Features.ServiceCategoryTest.CreateCategoryTest;

public class CreateCategoryHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CreateCategoryHandler _handler;
    
    public CreateCategoryHandlerTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new CreateCategoryHandler(_categoryRepositoryMock.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenCategoryIsCreated()
    {
        // Arrange
        var categoryName = "category1";
        var description = "this is cat";
        var mediaUrls = new List<string>
        {
            "https://example.com/image1.jpg",
            "https://example.com/image2.jpg"
        };
        
        var command = CreateCategoryCommand.Create(categoryName, description, mediaUrls);
        
        _categoryRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync(Result.Success());
        
        // Act
        var result = await _handler.HandleAsync(command.Data);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}