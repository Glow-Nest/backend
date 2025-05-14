using Domain.Aggregates.ProductReview;
using Domain.Aggregates.ProductReview.Values;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;

namespace DomainModelPersistence.Repositories;

public class ProductReviewRepository(DomainModelContext context) : RepositoryBase<ProductReview, ProductReviewId>(context), IProductReviewRepository
{
    
}