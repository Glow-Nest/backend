using Application.AppEntry;
using Application.AppEntry.Commands.Product;
using Domain.Aggregates.Product;
using OperationResult;

namespace Application.Handlers.ProductHandlers;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, None>
{
    private readonly IProductRepository _productRepository;
    
    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result<None>> HandleAsync(CreateProductCommand command)
    {
        var result = await Product.Create(command.name, command.price, command.imageUrl,command.description ,command.inventoryCount);
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric().ToNone();
        }

        var productAddResult = await _productRepository.AddAsync(result.Data);
        
        if (!productAddResult.IsSuccess)
        {
            return productAddResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}