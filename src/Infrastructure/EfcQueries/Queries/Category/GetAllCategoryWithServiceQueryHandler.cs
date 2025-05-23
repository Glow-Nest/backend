using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.Service;

namespace EfcQueries.Queries.Category;

public class GetAllCategoryWithServiceQueryHandler(PostgresContext context)
    : IQueryHandler<GetAllCategoriesWithServices.Query, Result<GetAllCategoriesWithServices.Answer>>
{
    private readonly PostgresContext _context = context;

    public async Task<Result<GetAllCategoriesWithServices.Answer>> HandleAsync(GetAllCategoriesWithServices.Query query)
    {
        var categories = await _context.Categories
            .Include(c => c.MediaUrls)
            .Include(c => c.Services)
            .ToListAsync();

        var categoryDtos = categories.Select(c => new GetAllCategoriesWithServices.CategoryWithServicesDto(
            c.CategoryId.ToString(),
            c.CategoryName,
            c.Description,
            c.MediaUrls.Select(m => m.Url).ToList(),
            c.Services.Select(s => new GetAllCategoriesWithServices.ServiceDto(
                s.ServiceId.ToString(),
                s.Name,
                s.Price,
                s.Duration.ToString()
            )).ToList()
        )).ToList();

        return Result<GetAllCategoriesWithServices.Answer>.Success(new GetAllCategoriesWithServices.Answer(categoryDtos));
    }
}