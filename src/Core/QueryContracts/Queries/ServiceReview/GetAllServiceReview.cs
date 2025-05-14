using OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.ServiceReview;

public class GetAllServiceReview
{
    public record ServiceReviewDto(string clientId, string serviceId, int rating, string reviewMessage);
    public record Query() : IQuery<Result<Answer>>;
    public record Answer(List<ServiceReviewDto> Reviews);
}