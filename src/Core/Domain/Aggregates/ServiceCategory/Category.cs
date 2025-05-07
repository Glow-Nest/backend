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
    
    //Update Category
    public Result UpdateCategory(CategoryName name, CategoryDescription description, List<MediaUrl> mediaUrls)
    {
        var nameResult = CategoryName.Create(name.Value);
        if (!nameResult.IsSuccess)
            return Result.Fail(nameResult.Errors);

        var descriptionResult = CategoryDescription.Create(description.Value);
        if (!descriptionResult.IsSuccess)
            return Result.Fail(descriptionResult.Errors);

        var newMediaUrls = new List<MediaUrl>();
        foreach (var url in mediaUrls)
        {
            var mediaUrlResult = MediaUrl.Create(url.Value);
            if (!mediaUrlResult.IsSuccess)
                return Result.Fail(mediaUrlResult.Errors);

            newMediaUrls.Add(mediaUrlResult.Data);
        }

        CategoryName = nameResult.Data;
        Description = descriptionResult.Data;
        MediaUrls = newMediaUrls; 

        return Result.Success();
    }
    
    //UpdateService
    public Result UpdateService(ServiceId serviceId, ServiceName name, Price price, TimeSpan duration)
    {
        var service = Services.FirstOrDefault(s => s.ServiceId.Equals(serviceId));
        if (service == null)
        {
            return Result.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        }

        var nameResult = ServiceName.Create(name.Value);
        if (!nameResult.IsSuccess)
            return Result.Fail(nameResult.Errors);

        var priceResult = Price.Create(price.Value);
        if (!priceResult.IsSuccess)
            return Result.Fail(priceResult.Errors);

        service.UpdateService(nameResult.Data, priceResult.Data, duration);

        return Result.Success();
    }
}