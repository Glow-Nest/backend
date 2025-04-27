using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;

namespace Services.Contracts.Schedule;

public class ServiceChecker(ICategoryRepository categoryRepository) : IServiceChecker
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<bool> DoesServiceExistsAsync(CategoryId categoryId, ServiceId serviceId)
    {
        var categoryResult = await _categoryRepository.FindServiceWithIdAsync(categoryId, serviceId);
        if (!categoryResult.IsSuccess)
        {
            return false;
        }
        
        return categoryResult.IsSuccess;
    }
}