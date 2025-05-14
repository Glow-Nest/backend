using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Product.Values;
using Domain.Aggregates.ProductReview.Values;
using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.ProductReview;

public class ProductReview : AggregateRoot
{
    internal ProductReviewId ProductReviewId { get; private set; }
    internal ProductId ProductId { get; private set; }
    internal ClientId ClientId { get; private set; }
    internal Rating Rating { get; private set; }
    internal ReviewMessage ReviewMessage { get; private set; }
    
    // For EFC
    private ProductReview()
    {
    }
    
    protected ProductReview(ProductReviewId productReviewId, ProductId productId, ClientId clientId, Rating rating, ReviewMessage reviewMessage)
    {
        ProductReviewId = productReviewId;
        ProductId = productId;
        ClientId = clientId;
        Rating = rating;
        ReviewMessage = reviewMessage;
    }
    
    public static async Task<Result<ProductReview>> Create(ProductId productId,
        ClientId clientId, Rating rating, ReviewMessage reviewMessage)
    {
        var reviewId = ProductReviewId.Create();
        var review = new ProductReview(reviewId, productId, clientId, rating, reviewMessage);
        return Result<ProductReview>.Success(review);
    }
    
}