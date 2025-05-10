using Domain.Common.OperationResult;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Product;

namespace EfcQueries.Queries.Product;

public class GetAllProductsQueryHandler(PostgresContext context) : IQueryHandler<GetAllProductsQuery.Query, Result<GetAllProductsQuery.Answer>>
{
    private readonly PostgresContext _context = context;

    public async Task<Result<GetAllProductsQuery.Answer>> HandleAsync(GetAllProductsQuery.Query query)
    {
        var products = await _context.Products
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new GetAllProductsQuery.ProductDto(
                p.ProductId.ToString(),
                p.Name,
                p.Price,
                p.ImageUrl))
            .ToListAsync();

        var totalCount = _context.Products.Count();

        return Result<GetAllProductsQuery.Answer>.Success(new GetAllProductsQuery.Answer(products, totalCount));
    }
}