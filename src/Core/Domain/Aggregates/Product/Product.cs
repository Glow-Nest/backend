using Domain.Aggregates.Product.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Product;

public class Product : AggregateRoot
{
    internal ProductId ProductId { get; }
    internal Name ProductName { get; private set; }
    internal Price Price { get; private set; }
    internal Description Description { get; private set; }
    internal InventoryCount InventoryCount { get; private set; }
    internal ImageUrl ImageUrl { get; private set; }

    private Product() // for efc
    {
        // For EFC
    }

    public Product(ProductId productId, Name productName, Price price, Description description, InventoryCount inventoryCount, ImageUrl imageUrl)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Description = description;
        InventoryCount = inventoryCount;
        ImageUrl = imageUrl;
    }

    public static async Task<Result<Product>> Create(Name name, Price price, ImageUrl imageUrl, Description description, InventoryCount inventoryCount )
    {
        var productId = ProductId.Create();
        var product = new Product(productId, name, price, description, inventoryCount,imageUrl);
        return Result<Product>.Success(product);
    }
}