using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using OperationResult;

namespace DomainModelPersistence.Repositories;

public class OrderRepository(DomainModelContext context) : RepositoryBase<Order, OrderId>(context), IOrderRepository
{
    public override async Task<Result<Order>> GetAsync(OrderId id)
    {
        var order = await context.Set<Order>()
            .Where(o => o.OrderId == id)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return Result<Order>.Fail(OrderErrorMessage.OrderNotFound());
        }

        return Result<Order>.Success(order);
    }
}