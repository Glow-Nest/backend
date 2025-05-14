using Domain.Aggregates.Product.Values;
using Domain.Common.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Product.UpdateProduct;

public class UpdateProductPriceCommand : UpdateProductCommandBase
{
    internal Price Price { get; }
    
    protected UpdateProductPriceCommand(ProductId id, Price price) : base(id)
    {
        Price = price;
    }
    
    public static Result<UpdateProductPriceCommand> Create(string id, double price)
    {
        var idResult = ProductId.FromGuid(Guid.Parse(id));
        
        var priceResult = Price.Create(price);
        if (!priceResult.IsSuccess)
        {
            return Result<UpdateProductPriceCommand>.Fail(priceResult.Errors);
        }
        
        return Result<UpdateProductPriceCommand>.Success(new UpdateProductPriceCommand(idResult, priceResult.Data));
    }
}