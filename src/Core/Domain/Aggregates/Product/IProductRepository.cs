using Domain.Aggregates.Product.Values;
using Domain.Common.Repositories;
using OperationResult;

namespace Domain.Aggregates.Product;

public interface IProductRepository : IGenericRepository<Product, ProductId>
{
    Task<Result> DeleteAsync(ProductId product);
    Task<Result<List<Product>>> GetProductsByIdsAsync(List<ProductId> productId);
}