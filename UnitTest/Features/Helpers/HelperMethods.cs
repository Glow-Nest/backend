using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.AppointmentValues;
using Domain.Aggregates.Service.Values;

namespace UnitTest.Features.Helpers;

public class HelperMethods
{
    public static CreateAppointmentDto CreateValidAppointmentDto()
    {
        return new CreateAppointmentDto(
            AppointmentNote.Create("Valid appointment note").Data,
            TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")).Data,
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            [ServiceId.Create()],
            ClientId.Create()
        );
    }
}