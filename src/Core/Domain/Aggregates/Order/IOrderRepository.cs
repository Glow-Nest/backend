using Domain.Aggregates.Order.Values;
using Domain.Common.Repositories;

namespace Domain.Aggregates.Order;

public interface IOrderRepository : IGenericRepository<Order, OrderId>
{
    
}