using Domain.Aggregates.Product.Values;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Product;

public class DeleteProductCommand
{
    public ProductId ProductId { get; }
    
    public DeleteProductCommand(ProductId productId)
    {
        ProductId = productId;
    }
    
    public static Result<DeleteProductCommand> Create(string productId)
    {
        if (!Guid.TryParse(productId, out Guid id))
        {
            return Result<DeleteProductCommand>.Fail(GenericErrorMessage.ErrorParsingGuid());
        }
        
        return Result<DeleteProductCommand>.Success(new DeleteProductCommand(ProductId.FromGuid(id)));
    }
}