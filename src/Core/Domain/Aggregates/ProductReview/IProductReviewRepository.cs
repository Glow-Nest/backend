using Domain.Aggregates.ProductReview.Values;
using Domain.Common.Repositories;

namespace Domain.Aggregates.ProductReview;

public interface IProductReviewRepository: IGenericRepository<ProductReview, ProductReviewId>
{
    
}