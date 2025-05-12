using Domain.Aggregates.Product;
using Domain.Common.OperationResult;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Product;

namespace EfcQueries.Queries.Product;

public class GetProductByNameQueryHandler(PostgresContext context) : IQueryHandler<GetProductByNameQuery.Query, Result<List<GetProductByNameQuery.Answer>>>
{
    private readonly PostgresContext _context = context;
    
    public async Task<Result<List<GetProductByNameQuery.Answer>>> HandleAsync(GetProductByNameQuery.Query query)
    {
        var products = await _context.Products
            .Where(p => EF.Functions.ILike(p.Name, $"%{query.ProductName}%"))
            .Select(p => new GetProductByNameQuery.Answer(
                p.ProductId.ToString(),
                p.Name,
                p.Price,
                p.ImageUrl))
            .ToListAsync();

        if (!products.Any())
        {
            return Result<List<GetProductByNameQuery.Answer>>.Fail(ProductErrorMessage.ProductNotFound());
        }

        return Result<List<GetProductByNameQuery.Answer>>.Success(products);
    }
}