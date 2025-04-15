using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Service.Values;
using Domain.Common.Repositories;

namespace Domain.Aggregates.Service;

public interface IServiceRepository : IGenericRepository<Service, ServiceId>
{
    
}