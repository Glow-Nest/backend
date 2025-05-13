using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Product;
using Domain.Aggregates.Product.Values;

namespace Services.Contracts.Order;

public class ProductChecker : IProductChecker
{
    private readonly IProductRepository _productRepository;

    public ProductChecker(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> DoesProductExist(ProductId productId)
    {
        var result = await _productRepository.GetAsync(productId);
        if (!result.IsSuccess)
        {
            return false;
        }

        return true;
    }
}