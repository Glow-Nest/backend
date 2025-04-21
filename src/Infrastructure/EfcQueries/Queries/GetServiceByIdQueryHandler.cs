using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Service;

namespace EfcQueries.Queries;

public class GetServiceByIdQueryHandler : IQueryHandler<GetServiceByIdQuery, Result<GetServiceByIdResponse>>
{
    
    private readonly DomainModelContext _context;

    public GetServiceByIdQueryHandler(DomainModelContext context)
    {
        _context = context;
    }
    
    public async Task<Result<GetServiceByIdResponse>> HandleAsync(GetServiceByIdQuery query)
    {
        var serviceId = ServiceId.FromGuid(query.ServiceId);
        var service = await _context.Set<Service>()
            .Where(s => s.ServiceId == serviceId) 
            .FirstOrDefaultAsync();
        
        if (service is null)
            return Result<GetServiceByIdResponse>.Fail(ServiceErrorMessage.ServiceNotFound());

        var dto = new ServiceDto(
            service.Name.Value,
            service.Description.Value,
            service.Price.Value,
            service.Duration.ToString(@"hh\:mm\:ss"),
            service.MediaUrls.Select(m => m.Value).ToList()
        );

        return Result<GetServiceByIdResponse>.Success(new GetServiceByIdResponse(dto));
    }
}