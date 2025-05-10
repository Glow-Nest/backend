using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.Service;

namespace EfcQueries.Queries.Category;

public class GetAllCategoryQueryHandler(ICategoryRepository categoryRepository) : IQueryHandler<GetAllCategory.Query,Result<GetAllCategory.Answer>>
{
    public async Task<Result<GetAllCategory.Answer>> HandleAsync(GetAllCategory.Query query)
    {
        var categories = await categoryRepository.GetAllAsync();
        
        if (!categories.IsSuccess)
        {
            return Result<GetAllCategory.Answer>.Fail(categories.Errors);
        }
        
        var categoryDtos = categories.Data.Select(c => new GetAllCategory.CategoryDto(
            c.CategoryId.Value.ToString(),
            c.CategoryName.Value,
            c.Description.Value,
            c.MediaUrls.Select(m => m.Value).ToList()
        )).ToList();
        
        return Result<GetAllCategory.Answer>.Success(new GetAllCategory.Answer(categoryDtos));
    }
}