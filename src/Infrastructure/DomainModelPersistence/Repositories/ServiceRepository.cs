using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.Repositories;

public class ServiceRepository(DbContext context) : RepositoryBase<Service, ServiceId>(context), IServiceRepository
{
    
}