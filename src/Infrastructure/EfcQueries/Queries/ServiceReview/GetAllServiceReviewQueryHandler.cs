using Domain.Aggregates.ServiceReview;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.ServiceReview;

namespace EfcQueries.Queries.ServiceReview;

public class GetAllServiceReviewQueryHandler(PostgresContext context) : IQueryHandler<GetAllServiceReview.Query, Result<GetAllServiceReview.Answer>>
{
    private readonly PostgresContext _context = context;
    
    public async Task<Result<GetAllServiceReview.Answer>> HandleAsync(GetAllServiceReview.Query query)
    {
        var reviews = await _context.ServiceReviews
            .Select(r => new GetAllServiceReview.ServiceReviewDto(
                r.ClientId.ToString(),
                r.ServiceId.ToString(),
                r.Rating,
                r.ReviewMessage))
            .ToListAsync();

        if (!reviews.Any())
        {
            return Result<GetAllServiceReview.Answer>.Fail(ServiceReviewErrorMessage.ServiceReviewNotFound());
        }

        return Result<GetAllServiceReview.Answer>.Success(new GetAllServiceReview.Answer(reviews));
    }
}