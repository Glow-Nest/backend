using Domain.Aggregates.Client;
using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Entities;
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

    public override async Task<Result<Category>> GetAsync(CategoryId id)
    {
        var category = await _context.Set<Category>()
            .Include(c => c.Services)
            .Include(c => c.MediaUrls)
            .FirstOrDefaultAsync(category => category.CategoryId == id);

        return category is null
            ? Result<Category>.Fail(ServiceCategoryErrorMessage.CategoryNotFound())
            : Result<Category>.Success(category);
    }

    public async Task<Result<List<Category>>> GetCategoriesWithServicesAsync()
    {
        var categories = await _context.Set<Category>()
            .Include(c => c.Services) 
            .Include(c => c.MediaUrls)
            .AsSplitQuery()
            .ToListAsync();
        
        return Result<List<Category>>.Success(categories);
    }

    public async Task<Result<bool>> FindServiceWithIdAsync(CategoryId categoryId, ServiceId serviceId)
    {
        var category = await _context.Set<Category>()
            .Include(c => c.Services)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

        if (category is null)
        {
            return Result<bool>.Fail(ServiceCategoryErrorMessage.CategoryNotFound());
        }

        var serviceExists = category.Services.Any(service => service.ServiceId.Equals(serviceId));

        return Result<bool>.Success(serviceExists);
    }

    public async Task<Result> DeleteAsync(CategoryId category)
    {
        var categoryToDelete = await _context.Set<Category>()
            .FirstOrDefaultAsync(c => c.CategoryId == category);
        if (categoryToDelete is null)
        {
            return  Result.Fail(ServiceCategoryErrorMessage.CategoryNotFound());
        }

        _context.Set<Category>().Remove(categoryToDelete);
        return Result.Success();
    }

    public async Task<Result> DeleteServiceAsync(CategoryId categoryId, ServiceId serviceId)
    {
        var categoryResult = await GetAsync(categoryId);
        if (!categoryResult.IsSuccess)
        {
            return Result.Fail(categoryResult.Errors);
        }
        
        var category = categoryResult.Data;
        var service = category.Services.FirstOrDefault(s => s.ServiceId.Equals(serviceId));
        
        if (service == null)
        {
            return Result.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        }
        
        category.Services.Remove(service);
        _context.Set<Service>().Remove(service);
        
        return Result.Success();
    }
}