using Application.AppEntry;
using Application.Common;
using Application.Interfaces;
using Domain.Aggregates.Client;
using Domain.Aggregates.Schedule.DomainEvents;
using Domain.Common.OperationResult;

namespace Application.Handlers.DomainEvents;

public class AppointmentCreatedDomainEventHandler(IClientRepository clientRepository, IEmailSender emailSender)
    : IDomainEventHandler<AppointmentCreatedDomainEvent>
{
    private IClientRepository _clientRepository = clientRepository;
    private IEmailSender _emailSender = emailSender;

    public async Task<Result> HandleAsync(AppointmentCreatedDomainEvent domainEvent)
    {
        // Get the client from the repository
        var clientResult = await _clientRepository.GetAsync(domainEvent.ClientId);

        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric();
        }

        // Send an email to the client
        var client = clientResult.Data;
        
        var message = $"Your appointment has been successfully created.\n\n" +
                      $"📅 Date: {domainEvent.AppointmentDate:dddd, MMMM d, yyyy}\n" +
                      $"⏰ Time: {domainEvent.TimeSlot.Start.ToString("hh:mm tt")} – {domainEvent.TimeSlot.End.ToString("hh:mm tt")}\n\n" +
                      "If you have any questions or need to make changes, feel free to contact us.\n\n";

        var sendEmailResult = await _emailSender.SendEmailAsync(client, EmailPurpose.AppointmentCreated,
            "Appointment Created",
            message);


        return Result.Success();
    }
}