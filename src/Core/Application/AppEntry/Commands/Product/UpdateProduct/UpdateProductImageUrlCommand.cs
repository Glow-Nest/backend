using Domain.Aggregates.Product.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Product.UpdateProduct;

public class UpdateProductImageUrlCommand : UpdateProductCommandBase
{
    internal ImageUrl ImageUrl { get; }
    
    protected UpdateProductImageUrlCommand(ProductId id, ImageUrl imageUrl) : base(id)
    {
        ImageUrl = imageUrl;
    }
    
    public static Result<UpdateProductImageUrlCommand> Create(string id, string imageUrl)
    {
        var idResult = ProductId.FromGuid(Guid.Parse(id));
        
        var imageUrlResult = ImageUrl.Create(imageUrl);
        if (!imageUrlResult.IsSuccess)
        {
            return Result<UpdateProductImageUrlCommand>.Fail(imageUrlResult.Errors);
        }
        
        return Result<UpdateProductImageUrlCommand>.Success(new UpdateProductImageUrlCommand(idResult, imageUrlResult.Data));
    }
}