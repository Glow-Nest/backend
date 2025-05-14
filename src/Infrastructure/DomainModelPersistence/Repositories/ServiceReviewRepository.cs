using Domain.Aggregates.ServiceReview;
using Domain.Aggregates.ServiceReview.Values;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using OperationResult;

namespace DomainModelPersistence.Repositories;

public class ServiceReviewRepository(DomainModelContext context) : RepositoryBase<ServiceReview, ServiceReviewId>(context), IServiceReviewRepository
{
}