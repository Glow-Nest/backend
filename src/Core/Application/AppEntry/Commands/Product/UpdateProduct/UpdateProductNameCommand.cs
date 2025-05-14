using Domain.Aggregates.Product.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Product.UpdateProduct;

public class UpdateProductNameCommand : UpdateProductCommandBase
{
    
    internal Name Name { get; }
    
    protected UpdateProductNameCommand(ProductId id, Name name) : base(id)
    {
        Name = name;
    }
    
    public static Result<UpdateProductNameCommand> Create(string id, string name)
    {
        var idResult = ProductId.FromGuid(Guid.Parse(id));
        
        var nameResult = Name.Create(name);
        if (!nameResult.IsSuccess)
        {
            return Result<UpdateProductNameCommand>.Fail(nameResult.Errors);
        }
        
        return Result<UpdateProductNameCommand>.Success(new UpdateProductNameCommand(idResult, nameResult.Data));
    }
}