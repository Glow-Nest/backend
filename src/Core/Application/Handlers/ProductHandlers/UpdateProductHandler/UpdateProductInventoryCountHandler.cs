using Application.AppEntry;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Domain.Aggregates.Product;
using Domain.Common.OperationResult;

namespace Application.Handlers.ProductHandlers.UpdateProductHandler;

public class UpdateProductInventoryCountHandler : ICommandHandler<UpdateProductInventoryCountCommand>
{
    private readonly IProductRepository _productRepository;
    
    public UpdateProductInventoryCountHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> HandleAsync(UpdateProductInventoryCountCommand command)
    {
        var findProductResult = await _productRepository.GetAsync(command.Id);
        
        if (!findProductResult.IsSuccess)
        {
            return findProductResult.ToNonGeneric();
        }

        var product = findProductResult.Data;
        var updateResult = product.UpdateInventoryCount(command.InventoryCount);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}