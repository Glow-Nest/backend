using Application.AppEntry;
using Application.AppEntry.Commands.ServiceReview;
using Domain.Aggregates.ServiceReview;
using OperationResult;

namespace Application.Handlers.ServiceReviewHandler;

public class CreateServiceReviewHandler : ICommandHandler<CreateServiceReviewCommand, None>
{
    private readonly IServiceReviewRepository _serviceReviewRepository;
    
    public CreateServiceReviewHandler(IServiceReviewRepository serviceReviewRepository)
    {
        _serviceReviewRepository = serviceReviewRepository;
    }
    
    public async Task<Result<None>> HandleAsync(CreateServiceReviewCommand command)
    {
        var result = await ServiceReview.Create(command.reviewedBy, command.rating, command.serviceId, command.ReviewMessage);
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric().ToNone();
        }
        
        var serviceReviewAddResult = await _serviceReviewRepository.AddAsync(result.Data);
        
        if (!serviceReviewAddResult.IsSuccess)
        {
            return serviceReviewAddResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}