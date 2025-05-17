
namespace Domain.Aggregates.Order.Values;

public enum OrderStatus
{
    Created = 1,
    Paid = 2,
    ReadyForPickup = 3,
    Completed = 4,
    Cancelled = 5
}