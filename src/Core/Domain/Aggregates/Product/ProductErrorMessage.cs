using Domain.Common.OperationResult;

namespace Domain.Aggregates.Product;

public class ProductErrorMessage
{
    public static Error EmptyProductName() => new Error("Product.EmptyName","Product name cannot be empty.");
    public static Error InvalidProductPrice() => new Error("Product.InvalidPrice", "Product price must be greater than zero.");
    public static Error EmptyDescription() => new Error("Product.EmptyDescription", "Product description cannot be empty.");
    public static Error EmptyImageUrl() => new Error("Product.EmptyImageUrl", "Product image URL cannot be empty.");
    public static Error InvalidInventoryCount() => new Error("Product.InvalidInventoryCount", "Product inventory count cannot be negative.");
}