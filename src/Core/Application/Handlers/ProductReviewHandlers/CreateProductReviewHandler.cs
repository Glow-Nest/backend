using Application.AppEntry;
using Application.AppEntry.Commands.ProductReview;
using Domain.Aggregates.ProductReview;
using OperationResult;

namespace Application.Handlers.ProductReviewHandlers;

public class CreateProductReviewHandler : ICommandHandler<CreateProductReviewCommand, None>
{
    private readonly IProductReviewRepository _productReviewRepository;
    
    public CreateProductReviewHandler(IProductReviewRepository productReviewRepository)
    {
        _productReviewRepository = productReviewRepository;
    }
    
    public async Task<Result<None>> HandleAsync(CreateProductReviewCommand command)
    {
        var result = await ProductReview.Create(command.productId, command.reviewedBy, command.rating,
            command.reviewMessage);
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric().ToNone();
        }
            
        var productReviewAddResult = await _productReviewRepository.AddAsync(result.Data);
        if (!productReviewAddResult.IsSuccess)
        {
            return productReviewAddResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}