using Application.AppEntry;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Domain.Aggregates.Product;
using Domain.Common.OperationResult;

namespace Application.Handlers.ProductHandlers.UpdateProductHandler;

public class UpdateProductPriceHandler :  ICommandHandler<UpdateProductPriceCommand>
{
    
    private readonly IProductRepository _productRepository;
    
    public UpdateProductPriceHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result> HandleAsync(UpdateProductPriceCommand command)
    {
        var findProductResult = await _productRepository.GetAsync(command.Id);
        
        if (!findProductResult.IsSuccess)
        {
            return findProductResult.ToNonGeneric();
        }

        var product = findProductResult.Data;
        var updateResult = product.UpdatePrice(command.Price);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}