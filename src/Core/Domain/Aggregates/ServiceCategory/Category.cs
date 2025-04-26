using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.ServiceCategory;

public class Category : AggregateRoot
{
    internal CategoryId CategoryId { get; }
    internal CategoryName CategoryName { get; private set; }
    internal CategoryDescription Description { get; private set; }
    internal List<MediaUrl> MediaUrls { get; private set; }
    internal List<Entities.Service> _services { get; private set; } = new();
    
    private Category() // for efc
    {
        // For EFC
    }
    private Category(CategoryId categoryId, CategoryName categoryName, CategoryDescription description, List<MediaUrl> mediaUrls)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        Description = description;
        MediaUrls = mediaUrls;
    }
    
    public static async Task<Result<Category>> Create(CategoryName name, CategoryDescription description, List<MediaUrl> mediaUrls)
    {
        var categoryId = CategoryId.Create();
        var category = new Category(categoryId, name, description, mediaUrls);
        return Result<Category>.Success(category);
    }
    
    //AddService
    public async Task<Result<Entities.Service>> AddService(ServiceName name, Price price, TimeSpan duration)
    {
        var serviceResult = await Entities.Service.Create(name, price, duration);
        if (!serviceResult.IsSuccess)
        {
            return Result<Entities.Service>.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        }
        
        var service = serviceResult.Data;
        _services.Add(service);
        
        return Result<Entities.Service>.Success(service);
    }
    
}