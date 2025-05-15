using Domain.Aggregates.Product.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Product.UpdateProduct;

public class UpdateProductDescriptionCommand  : UpdateProductCommandBase
{
    internal Description Description { get; }
    
    protected UpdateProductDescriptionCommand(ProductId id, Description description) : base(id)
    {
        Description = description;
    }
    
    public static Result<UpdateProductDescriptionCommand> Create(string id, string description)
    {
        var idResult = ProductId.FromGuid(Guid.Parse(id));
        
        var descriptionResult = Description.Create(description);
        if (!descriptionResult.IsSuccess)
        {
            return Result<UpdateProductDescriptionCommand>.Fail(descriptionResult.Errors);
        }
        
        return Result<UpdateProductDescriptionCommand>.Success(new UpdateProductDescriptionCommand(idResult, descriptionResult.Data));
    }
}