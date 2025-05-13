using Domain.Aggregates.Product.Values;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;
using Domain.Common.Repositories;

namespace Domain.Aggregates.Product;

public interface IProductRepository : IGenericRepository<Product, ProductId>
{
    Task<Result> DeleteAsync(ProductId product);
}