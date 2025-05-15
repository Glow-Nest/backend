using Application.AppEntry;
using Application.AppEntry.Commands.Product;
using Domain.Aggregates.Product;
using OperationResult;

namespace Application.Handlers.ProductHandlers;

public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, None>
{
    private readonly IProductRepository _productRepository;
    
    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result<None>> HandleAsync(DeleteProductCommand command)
    {
        var findProductResult = await _productRepository.GetAsync(command.ProductId);
        
        if (!findProductResult.IsSuccess)
        {
            return findProductResult.ToNonGeneric().ToNone();
        }

        var deleteResult = await _productRepository.DeleteAsync(command.ProductId);
        
        if (!deleteResult.IsSuccess)
        {
            return deleteResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}