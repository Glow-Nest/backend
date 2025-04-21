using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Service;


public record ServiceDto(string name, string description, double price, string duration, List<string> mediaUrls);

public record GetAllServicesResponse(List<ServiceDto> Services);

public record GetAllServiceQuery : IQuery<Result<GetAllServicesResponse>>;
