using Domain.Aggregates.Product;
using Domain.Common.OperationResult;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Product;

namespace EfcQueries.Queries.Product;

public class GetProductByNameQueryHandler(PostgresContext context) : IQueryHandler<GetProductByNameQuery.Query,Result<GetProductByNameQuery.Answer>>
{
    private readonly PostgresContext _context = context;
    
    public async Task<Result<GetProductByNameQuery.Answer>> HandleAsync(GetProductByNameQuery.Query query)
    {
        var product = await _context.Products
            .Where(product => product.Name.Contains(query.ProductName))
            .Select(product => new GetProductByNameQuery.Answer(
                product.ProductId.ToString(),
                product.Name))
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return Result<GetProductByNameQuery.Answer>.Fail(ProductErrorMessage.ProductNotFound());
        }
        
        return Result<GetProductByNameQuery.Answer>.Success(product);
    }
}