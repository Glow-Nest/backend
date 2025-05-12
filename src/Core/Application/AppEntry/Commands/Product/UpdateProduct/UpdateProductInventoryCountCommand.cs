using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Domain.Aggregates.Product.Values;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Product.UpdateProduct;

public class UpdateProductInventoryCountCommand : UpdateProductCommandBase
{
    internal InventoryCount InventoryCount { get; }
    protected UpdateProductInventoryCountCommand(ProductId id, InventoryCount inventoryCount) : base(id)
    {
        InventoryCount = inventoryCount;
    }
    
    public static Result<UpdateProductInventoryCountCommand> Create(string id, int inventoryCount)
    {
        var idResult = ProductId.FromGuid(Guid.Parse(id));
        
        var inventoryCountResult = InventoryCount.Create(inventoryCount);
        if (!inventoryCountResult.IsSuccess)
        {
            return Result<UpdateProductInventoryCountCommand>.Fail(inventoryCountResult.Errors);
        }
        
        return Result<UpdateProductInventoryCountCommand>.Success(new UpdateProductInventoryCountCommand(idResult, inventoryCountResult.Data));
    }
}