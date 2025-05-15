using Domain.Aggregates.ServiceReview.Values;
using Domain.Common.Repositories;
using OperationResult;

namespace Domain.Aggregates.ServiceReview;

public interface IServiceReviewRepository  : IGenericRepository<ServiceReview, ServiceReviewId>
{
}