using Domain.Aggregates.Appointment.Values;
using Domain.Common.Repositories;

namespace Domain.Aggregates.Appointment;

public interface IAppointmentRepository : IGenericRepository<Appointment, AppointmentId>
{
    
}