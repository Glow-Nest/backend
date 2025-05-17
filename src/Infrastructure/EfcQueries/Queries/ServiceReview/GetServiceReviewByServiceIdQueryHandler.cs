using Domain.Aggregates.ServiceReview;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.ServiceReview;

namespace EfcQueries.Queries.ServiceReview;

public class GetServiceReviewByServiceIdQueryHandler(PostgresContext context) : IQueryHandler<GetServiceReviewByServiceId.Query, Result<GetServiceReviewByServiceId.Answer>>
{
    private readonly PostgresContext _context = context;
    
    public async Task<Result<GetServiceReviewByServiceId.Answer>> HandleAsync(GetServiceReviewByServiceId.Query query)
    {
        var reviews = await _context.ServiceReviews.Where(rev => rev.ServiceId.ToString() == query.ServiceId)
            .Select(r => new GetServiceReviewByServiceId.ServiceReviewDto(
                r.ServiceReviewId.ToString(),
                r.ClientId.ToString(),
                r.ServiceId.ToString(),
                r.Rating,
                r.ReviewMessage))
            .ToListAsync();

        if (!reviews.Any())
        {
            return Result<GetServiceReviewByServiceId.Answer>.Fail(ServiceReviewErrorMessage.ServiceReviewNotFound());
        }

        return Result<GetServiceReviewByServiceId.Answer>.Success(new GetServiceReviewByServiceId.Answer(reviews));
    }
}