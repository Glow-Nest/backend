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
    public static Error InvalidOrderId() => new Error("Order.InvalidOrderId", "Order id is not valid.");
    public static Error OrderNotFound() => new Error("Order.OrderNotFound", "Order not found.");
    public static Error OrderNotCreatedState() => new Error("Order.NotCreatedState", "Order can only be marked as paid from the created state.");
    public static Error OrderNotPaidState() => new Error("Order.NotPaidState", "Order must be paid before it can be marked as ready for pickup.");
    public static Error OrderNotReadyForPickupState() => new Error("Order.NotReadyForPickupState", "Order must be ready for pickup before it can be marked as completed.");
    public static Error OrderCannotBeCancelled() => new Error("Order.OrderCannotBeCancelled", "Order cannot be cancelled.");
    public static Error PickupDateInThePast() => new Error("Order.PickupDateInThePast", "Pickup date cannot be in the past.");
    public static Error PickupDateTooLateForToday() => new Error("Order.PickupDateTooLateForToday", "Pickup date cannot be later than 03:00 PM today.");
    public static Error PriceCanNotBeNegative() => new Error("Order.PriceCanNotBeNegative", "Price cannot be negative or 0.");
    public static Error ProductDoesNotExist() => new Error("Order.ProductDoesNotExist", "Product does not exist.");
    public static Error NoOrderItems() => new Error("Order.NoOrderItems", "Order must have at least one order item.");
    public static Error OrderCannotChangePickupDate() => new Error("Order.OrderCannotChangePickupDate", "Order cannot change pickup date.");
    public static Error OrderCannotChangeOrderItems() => new Error("Order.OrderCannotChangeOrderItems", "Order cannot change order items.");
    
    // Stripe
    public static Error StripePaymentIntentNotFound() => new Error("Order.StripePaymentIntentNotFound", "Stripe payment intent not found.");
}