using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Values;
using OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;

public class UpdateServicePriceCommand : UpdateServiceCommandBase
{
    internal Price Price { get; }
    
    protected UpdateServicePriceCommand(CategoryId categoryId, ServiceId serviceId, Price price) : base(categoryId, serviceId)
    {
        Price = price;
    }
    
    public static Result<UpdateServicePriceCommand> Create(string categoryId, string serviceId, double price)
    {
        var categoryIdResult = CategoryId.FromGuid(Guid.Parse(categoryId));
        var serviceIdResult = ServiceId.FromGuid(Guid.Parse(serviceId));
        
        var priceResult = Price.Create(price);
        if (!priceResult.IsSuccess)
        {
            return Result<UpdateServicePriceCommand>.Fail(priceResult.Errors);
        }
        
        return Result<UpdateServicePriceCommand>.Success(new UpdateServicePriceCommand(categoryIdResult, serviceIdResult, priceResult.Data));
    }
}