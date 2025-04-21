using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.Repositories;

public class ServiceRepository(DomainModelContext context) : RepositoryBase<Service, ServiceId>(context), IServiceRepository
{

}