using Domain.Aggregates.Client;
using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.Repositories;

public class CategoryRepository: RepositoryBase<Category,CategoryId>, ICategoryRepository
{
    private readonly DomainModelContext _context;
    
    public CategoryRepository(DomainModelContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Category>>> GetAllAsync()
    {
        var categories = await _context.Set<Category>().ToListAsync();
        return Result<List<Category>>.Success(categories);
    }

    public async Task<Result<List<Category>>> GetCategoriesWithServicesAsync()
    {
        var categories = await _context.Set<Category>()
            .Include(c => c._services) 
            .Include(c => c.MediaUrls)
            .ToListAsync();
        
        return Result<List<Category>>.Success(categories);
    }
}