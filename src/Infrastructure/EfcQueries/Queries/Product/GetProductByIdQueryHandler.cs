using Domain.Aggregates.Product;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.Product;

namespace EfcQueries.Queries.Product;

public class GetProductByIdQueryHandler(PostgresContext context) : IQueryHandler<GetProductByIdQuery.Query, Result<GetProductByIdQuery.Answer>>
{
    private readonly PostgresContext _context = context;

    public async Task<Result<GetProductByIdQuery.Answer>> HandleAsync(GetProductByIdQuery.Query query)
    {
        var product = await _context.Products.Where(product => product.ProductId.ToString() == query.ProductId)
            .Select(product => new GetProductByIdQuery.Answer(
                product.ProductId.ToString(),
                product.Name,
                product.Description,
                product.Price,
                product.ImageUrl,
                product.InventoryCount))
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return Result<GetProductByIdQuery.Answer>.Fail(ProductErrorMessage.ProductNotFound());
        }
        
        return Result<GetProductByIdQuery.Answer>.Success(product);
    }
}