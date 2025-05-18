using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.Order;

namespace EfcQueries.Queries.Order;

public class GetAllOrdersQueryHandler(PostgresContext context)
    : IQueryHandler<GetAllOrdersQuery.Query, Result<GetAllOrdersQuery.Answer>>
{
    private readonly PostgresContext _context = context;

    public async Task<Result<GetAllOrdersQuery.Answer>> HandleAsync(GetAllOrdersQuery.Query query)
    {
        var orders = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => string.IsNullOrEmpty(query.Status) || query.Status == "All" || o.OrderStatus == query.Status)            .Select(o => new GetAllOrdersQuery.OrderResponseDto(
                o.OrderId.ToString(),
                o.OrderDate,
                o.PickupDate,
                o.OrderStatus,
                o.TotalPrice.ToString(),
                $"{o.Client.FirstName} {o.Client.LastName}",
                o.OrderItems.Select(oi => new GetAllOrdersQuery.OrderItemResponseDto(
                    oi.ProductId.ToString(),
                    oi.Product.Name,
                    oi.Quantity.ToString(),
                    oi.PriceWhenOrdering.ToString()
                )).ToList()
            )).ToListAsync();
        
        return Result<GetAllOrdersQuery.Answer>.Success(new GetAllOrdersQuery.Answer(orders));
    }
}