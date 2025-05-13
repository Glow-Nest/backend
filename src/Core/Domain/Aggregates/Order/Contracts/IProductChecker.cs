using Domain.Aggregates.Product.Values;

namespace Domain.Aggregates.Order.Contracts;

public interface IProductChecker
{
    Task<bool> DoesProductExist(ProductId productId);
}