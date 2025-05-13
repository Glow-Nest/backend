using OperationResult;

namespace Domain.Aggregates.Order;

public class OrderErrorMessage
{
    public static Error EmptyQuantity() => new Error("Order.EmptyQuantity", "Quantity must be greater than 0.");
    public static Error EmptyPrice() => new Error("Order.EmptyPrice", "Price must be greater than 0.");
    public static Error EmptyOrderId() => new Error("Order.EmptyOrderId", "Order id cannot be empty.");

    // Order Item
    public static Error EmptyOrderItemId() => new Error("Order.EmptyOrderItemId", "Order item id cannot be empty.");
    
    
    // Order
    public static Error PickupDateInThePast() => new Error("Order.PickupDateInThePast", "Pickup date cannot be in the past.");
}