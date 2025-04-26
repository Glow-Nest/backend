using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;
using Domain.Common.Repositories;

namespace Domain.Aggregates.ServiceCategory;

public interface ICategoryRepository : IGenericRepository<Category, CategoryId>
{
    Task<Result<List<Category>>> GetAllAsync();
    Task<Result<List<Category>>> GetCategoriesWithServicesAsync();
}