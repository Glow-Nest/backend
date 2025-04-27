using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Service;

public class GetAllCategoriesWithServices
{
    public record ServiceDto(string ServiceId, string Name, double Price, string Duration);
    public record CategoryWithServicesDto(
        string CategoryId,
        string Name,
        string Description,
        List<string> MediaUrls,
        List<ServiceDto> Services
    );
    
    public record Query() : IQuery<Result<Answer>>;
    public record Answer(List<CategoryWithServicesDto> Categories);

    
}