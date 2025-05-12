using Domain.Aggregates.Product.Values;

namespace Application.AppEntry.Commands.Product.UpdateProduct;

public class UpdateProductCommandBase
{
    internal ProductId Id { get; }
    
    protected UpdateProductCommandBase(ProductId id)
    {
        Id = id;
    }
}