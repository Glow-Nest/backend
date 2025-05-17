using OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.ServiceReview;

public class GetServiceReviewByServiceId
{
    public record ServiceReviewDto(string ServiceReviewId, string UserId, string ServiceId, int Rating, string ReviewMessage);
    public record Query(string ServiceId) : IQuery<Result<Answer>>;
    public record Answer(List<ServiceReviewDto> Reviews);
}