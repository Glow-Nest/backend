using Application.AppEntry;
using Application.AppEntry.Commands.Product;
using Domain.Aggregates.Product;
using Domain.Common.OperationResult;

namespace Application.Handlers.ProductHandlers;

public class CreateProductHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    
    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result> HandleAsync(CreateProductCommand command)
    {
        var result = await Product.Create(command.name, command.price, command.imageUrl,command.description ,command.inventoryCount);
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric();
        }

        var productAddResult = await _productRepository.AddAsync(result.Data);
        
        if (!productAddResult.IsSuccess)
        {
            return productAddResult;
        }
        
        return Result.Success();
    }
}