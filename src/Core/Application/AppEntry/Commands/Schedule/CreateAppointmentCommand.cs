using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.AppointmentValues;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common;
using OperationResult;

namespace Application.AppEntry.Commands.Schedule;

public class CreateAppointmentCommand(AppointmentNote appointmentNote, TimeSlot timeSlot, List<ServiceId> serviceIds, List<CategoryId> categoryIds, Email email, DateOnly appointmentDate)
{
    internal readonly AppointmentNote appointmentNote = appointmentNote;
    internal readonly TimeSlot timeSlot = timeSlot;
    internal readonly DateOnly appointmentDate = appointmentDate;
    internal readonly List<ServiceId> serviceIds = serviceIds;
    internal readonly List<CategoryId> categoryIds = categoryIds;
    internal readonly Email email = email;

    public static Result<CreateAppointmentCommand> Create(string appointmentNote, string startTime, string endTime,
        string appointmentDate, List<string> servicesIdsStr, List<string> categoryIdsStr, string clientId)
    {
        var listOfErrors = new List<Error>();
        
        // appointment note
        var appointmentNoteResult = AppointmentNote.Create(appointmentNote);
        if (!appointmentNoteResult.IsSuccess)
        {
            listOfErrors.AddRange(appointmentNoteResult.Errors);
        }
        
        // time slot
        var startTimeResult = TimeOnly.TryParse(startTime, out var startTimeParsed);
        var endTimeResult = TimeOnly.TryParse(endTime, out var endTimeParsed);
        
        if (!startTimeResult || !endTimeResult)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingTime());
        }
        
        var timeSlotResult = TimeSlot.Create(startTimeParsed, endTimeParsed);
        if (!timeSlotResult.IsSuccess)
        {
            listOfErrors.AddRange(timeSlotResult.Errors);
        }
        
        // appointment date
        var appointmentDateResult = DateOnly.TryParse(appointmentDate, out var appointmentDateParsed);
        if (!appointmentDateResult)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingDate());
        }
        
        // services ids
        var serviceIds = new List<ServiceId>();
        foreach (var serviceId in servicesIdsStr)
        {
            var idParseResult = Guid.TryParse(serviceId, out var guid);
            
            if (!idParseResult)
            {
                listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
            }

            var id = ServiceId.FromGuid(guid);
            serviceIds.Add(id);
        }
        
        // category ids
        var categoryIds = new List<CategoryId>();
        foreach (var categoryId in categoryIdsStr)
        {
            var idParseResult = Guid.TryParse(categoryId, out var guid);
            if (!idParseResult)
            {
                listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
            }
            
            var id = CategoryId.FromGuid(guid);
            categoryIds.Add(id);
        }
        
        // client id
        /*var clientIdParseResult = Guid.TryParse(clientId, out var clientGuid);
        if (!clientIdParseResult)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
        }
        var clientIdResult = ClientId.FromGuid(clientGuid);*/
        
        var emailResult = Email.Create(clientId);
        if (!emailResult.IsSuccess)
        {
            listOfErrors.AddRange(emailResult.Errors);
        }

        if (listOfErrors.Any())
        {
            return Result<CreateAppointmentCommand>.Fail(listOfErrors);
        }
        
        var command = new CreateAppointmentCommand(appointmentNoteResult.Data, timeSlotResult.Data,
             serviceIds, categoryIds, emailResult.Data, appointmentDateParsed);
        return Result<CreateAppointmentCommand>.Success(command);
    }
}