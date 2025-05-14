using OperationResult;

namespace Domain.Aggregates.Product;

public class ProductErrorMessage
{
    public static Error EmptyProductName() => new Error("Product.EmptyName","Product name cannot be empty.");
    public static Error InvalidPrice() => new Error("Product.InvalidPrice", "Price must be greater than zero.");
    public static Error EmptyDescription() => new Error("Product.EmptyDescription", "Product description cannot be empty.");
    public static Error EmptyImageUrl() => new Error("Product.EmptyImageUrl", "Product image URL cannot be empty.");
    public static Error InvalidInventoryCount() => new Error("Product.InvalidInventoryCount", "Product inventory count cannot be negative.");
    public static Error ProductNotFound() => new Error("Product.NotFound", "Product not found.");
}