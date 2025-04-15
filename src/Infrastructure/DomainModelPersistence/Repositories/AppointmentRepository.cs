using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Values;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment, AppointmentId>, IAppointmentRepository
{
    private readonly DomainModelContext _context;

    public AppointmentRepository(DomainModelContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<Result<Appointment>> GetAsync(AppointmentId id)
    {
        var appointment = await _context.Set<Appointment>()
            .Include(appointment => appointment.Services)
            .FirstOrDefaultAsync(appointment => appointment.AppointmentId == id);

        return appointment is null
            ? Result<Appointment>.Fail(AppointmentErrorMessage.AppointmentNotFound())
            : Result<Appointment>.Success(appointment);
    }
}