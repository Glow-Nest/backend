using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Service;

public class GetAllCategory
{
    public record CategoryDto(string CategoryId, string Name, string Description, List<string> MediaUrls);
    public record Query() : IQuery<Result<Answer>>;
    public record Answer(List<CategoryDto> Categories);

}

