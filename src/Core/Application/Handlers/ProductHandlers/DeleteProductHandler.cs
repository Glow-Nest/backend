using Application.AppEntry;
using Application.AppEntry.Commands.Product;
using Domain.Aggregates.Product;
using Domain.Common.OperationResult;

namespace Application.Handlers.ProductHandlers;

public class DeleteProductHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    
    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Result> HandleAsync(DeleteProductCommand command)
    {
        var findProductResult = await _productRepository.GetAsync(command.ProductId);
        
        if (!findProductResult.IsSuccess)
        {
            return findProductResult.ToNonGeneric();
        }

        var deleteResult = await _productRepository.DeleteAsync(command.ProductId);
        
        if (!deleteResult.IsSuccess)
        {
            return deleteResult;
        }
        
        return Result.Success();
    }
}