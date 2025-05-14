using Domain.Aggregates.Product;
using Domain.Aggregates.Product.Values;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

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
}