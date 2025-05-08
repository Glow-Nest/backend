using Domain.Aggregates.Product;
using Domain.Aggregates.Product.Values;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.Repositories;

public class ProductRepository(DomainModelContext context) : RepositoryBase<Product, ProductId>(context), IProductRepository
{
    
}