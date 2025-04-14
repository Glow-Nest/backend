using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Values;
using DomainModelPersistence.Common;
using DomainModelPersistence.EfcConfigs;

namespace DomainModelPersistence.AppointmentPersistence;

public class AppointmentRepository : RepositoryBase<Appointment, AppointmentId>, IAppointmentRepository
{
    private readonly DomainModelContext _context;
    
    public AppointmentRepository(DomainModelContext context) : base(context)
    {
        _context = context;
    }
    
    
}