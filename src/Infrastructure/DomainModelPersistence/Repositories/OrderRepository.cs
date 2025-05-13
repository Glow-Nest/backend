using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;

namespace DomainModelPersistence.Repositories;

public class OrderRepository(DomainModelContext context) : RepositoryBase<Order, OrderId>(context), IOrderRepository
{
    
}