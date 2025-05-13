using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.ServiceCategory;

public class Category : AggregateRoot
{
    internal CategoryId CategoryId { get; }
    internal CategoryName CategoryName { get; private set; }
    internal CategoryDescription Description { get; private set; }
    internal List<MediaUrl> MediaUrls { get; private set; }
    internal List<Service> Services { get;} = new();
    
    private Category() // for efc
    {
        // For EFC
    }
    private Category(CategoryId categoryId, CategoryName categoryName, CategoryDescription description, List<MediaUrl>? mediaUrls)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        Description = description;
        MediaUrls = mediaUrls?? new List<MediaUrl>();
    }
    
    public static async Task<Result<Category>> Create(CategoryName name, CategoryDescription description, List<MediaUrl>? mediaUrls)
    {
        var categoryId = CategoryId.Create();
        var category = new Category(categoryId, name, description, mediaUrls);
        return Result<Category>.Success(category);
    }
    
    //AddService
    public async Task<Result<Service>> AddService(ServiceName name, Price price, TimeSpan duration)
    {
        var serviceResult = await Service.Create(name, price, duration);
        if (!serviceResult.IsSuccess)
        {
            return Result<Service>.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        }
        
        var service = serviceResult.Data;
        Services.Add(service);
        
        return Result<Service>.Success(service);
    }

    public Result UpdateCategoryName(CategoryName name)
    {
        CategoryName = name;
        return Result.Success();
    }
    
    public Result UpdateCategoryDescription(CategoryDescription description)
    {
        Description = description;
        return Result.Success();
    }
    
    public Result UpdateCategoryMediaUrls(List<MediaUrl> mediaUrls)
    {
        MediaUrls = mediaUrls;
        return Result.Success();
    }
    
    public Result UpdateServiceName(ServiceId serviceId, ServiceName name)
    {
        var service = Services.FirstOrDefault(x => x.ServiceId.Equals(serviceId));
        if (service == null)
            return Result.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        
        return service.UpdateServiceName(name);
    }
    
    public Result UpdateServicePrice(ServiceId serviceId, Price price)
    {
        var service = Services.FirstOrDefault(x => x.ServiceId.Equals(serviceId));
        if (service == null)
            return Result.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        
        return service.UpdateServicePrice(price);
    }
    
    public Result UpdateServiceDuration(ServiceId serviceId, TimeSpan duration)
    {
        var service = Services.FirstOrDefault(x => x.ServiceId.Equals(serviceId));
        if (service == null)
            return Result.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        
        return service.UpdateServiceDuration(duration);
    }
}