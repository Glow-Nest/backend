using Domain.Aggregates.Service;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Service;

namespace EfcQueries.Queries;

public class GetAllServicesQueryHandler : IQueryHandler<GetAllServiceQuery, Result<GetAllServicesResponse>>
{
    
    private readonly DomainModelContext _context;
    
    public GetAllServicesQueryHandler(DomainModelContext context)
    {
        _context = context;
    }
    
    public async Task<Result<GetAllServicesResponse>> HandleAsync(GetAllServiceQuery query)
    {
        var services = await _context.Set<Service>()
            // .Include(s => s.MediaUrls) 
            .ToListAsync();
        
        var serviceDtos = services.Select(s => new ServiceDto(
            s.ServiceId.Value,
            s.Name.Value,
            s.Description.Value,
            s.Price.Value,
            s.Duration.ToString(@"hh\:mm\:ss"),
            s.MediaUrls.Select(m => m.Value).ToList()
        )).ToList();

        return Result<GetAllServicesResponse>.Success(new GetAllServicesResponse(serviceDtos));
    }
}