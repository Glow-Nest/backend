using Domain.Aggregates.Product;
using Domain.Aggregates.Product.Values;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using OperationResult;

namespace DomainModelPersistence.Repositories;

public class ProductRepository(DomainModelContext context) : RepositoryBase<Product, ProductId>(context), IProductRepository
{
    public async Task<Result> DeleteAsync(ProductId product)
    {
        var productToDelete = await context.Set<Product>()
            .FirstOrDefaultAsync(pro => pro.ProductId == product);

        if (productToDelete == null)
        {
            return  Result.Fail(ProductErrorMessage.ProductNotFound());
        }
        
        context.Set<Product>().Remove(productToDelete);
        return Result.Success();
    }

    public async Task<Result<List<Product>>> GetProductsByIdsAsync(List<ProductId> productIds)
    {
        var allProduct = await context.Set<Product>().Where(pro => productIds.Contains(pro.ProductId))
            .ToListAsync();
        if (allProduct.Count == 0)
        {
            return Result<List<Product>>.Fail(ProductErrorMessage.ProductNotFound());
        }
        
        return Result<List<Product>>.Success(allProduct);
    }
}