using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Service;

namespace EfcQueries.Queries;

public class GetAllCategoryWithServiceQueryHandler (ICategoryRepository categoryRepository) : IQueryHandler<GetAllCategoriesWithServices.Query,Result<GetAllCategoriesWithServices.Answer>>
{

    public async Task<Result<GetAllCategoriesWithServices.Answer>> HandleAsync(GetAllCategoriesWithServices.Query query)
    {
        var categoriesResult = await categoryRepository.GetCategoriesWithServicesAsync();
        
        if (!categoriesResult.IsSuccess)
        {
            return Result<GetAllCategoriesWithServices.Answer>.Fail(categoriesResult.Errors);
        }
        
        var categories = categoriesResult.Data;
        var categoryDtos = categories.Select(c => new GetAllCategoriesWithServices.CategoryWithServicesDto(
            c.CategoryId.Value.ToString(),
            c.CategoryName.Value,
            c.Description.Value,
            c.MediaUrls.Select(m => m.Value).ToList(),
            c.Services.Select(s => new GetAllCategoriesWithServices.ServiceDto(
                s.ServiceId.Value.ToString(),
                s.Name.Value,
                s.Price.Value,
                s.Duration.ToString()
            )).ToList()
        )).ToList();
        
        return Result<GetAllCategoriesWithServices.Answer>.Success(new GetAllCategoriesWithServices.Answer(categoryDtos));
    }
}