using Domain.Aggregates.Product.Values;
using Domain.Common.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Product;

public class CreateProductCommand (Name name, Price price, Description description, ImageUrl imageUrl, InventoryCount inventoryCount)
{
    internal readonly Name name = name;
    internal readonly Price price = price;
    internal readonly Description description = description;
    internal readonly ImageUrl imageUrl = imageUrl;
    internal readonly InventoryCount inventoryCount = inventoryCount;

    public static Result<CreateProductCommand> Create(string name, double price, string description, string imageUrl,
        int inventoryCount)
    {
        var listOfErrors = new List<Error>();
        var nameResult = Name.Create(name);
        if (!nameResult.IsSuccess)
        {
            listOfErrors.AddRange(nameResult.Errors);
        }
        
        var priceResult = Price.Create(price);
        if (!priceResult.IsSuccess)
        {
            listOfErrors.AddRange(priceResult.Errors);
        }
        var descriptionResult = Description.Create(description);
        if (!descriptionResult.IsSuccess)
        {
            listOfErrors.AddRange(descriptionResult.Errors);
        }
        
        var imageUrlResult = ImageUrl.Create(imageUrl);
        if (!imageUrlResult.IsSuccess)
        {
            listOfErrors.AddRange(imageUrlResult.Errors);
        }
        
        var inventoryCountResult = InventoryCount.Create(inventoryCount);
        if (!inventoryCountResult.IsSuccess)
        {
            listOfErrors.AddRange(inventoryCountResult.Errors);
        }
        
        if (listOfErrors.Any())
        {
            return Result<CreateProductCommand>.Fail(listOfErrors);
        }
        
        var command = new CreateProductCommand(nameResult.Data, priceResult.Data, descriptionResult.Data,
            imageUrlResult.Data, inventoryCountResult.Data);
        
        return Result<CreateProductCommand>.Success(command);
    }
    
}