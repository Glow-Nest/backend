using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Service;

public record GetServiceByIdResponse(ServiceDto Services);

public record GetServiceByIdQuery(Guid ServiceId) : IQuery<Result<GetServiceByIdResponse>>;