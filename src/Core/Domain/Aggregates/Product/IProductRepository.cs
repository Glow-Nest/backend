using Domain.Aggregates.Product.Values;
using Domain.Common.Repositories;

namespace Domain.Aggregates.Product;

public interface IProductRepository : IGenericRepository<Product, ProductId>
{
    
}