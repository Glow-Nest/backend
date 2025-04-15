using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Domain.Common.OperationResult;
using DomainModelPersistence.ClientPersistence;
using DomainModelPersistence.Repositories;
using IntegrationTests.Helpers;
using Moq;
using UnitTest.Features.Helpers.Builders;

namespace IntegrationTests.RepositoriesTest;

public class AppointmentRepoTest
{
    [Fact]
    public async Task SaveAndRetrieveAppointment_ShouldMatchOriginal()
    {
        // Arrange
        var serviceChecker = new Mock<IServiceChecker>();
        serviceChecker.Setup(x => x.DoesServiceExistsAsync(It.IsAny<ServiceId>()))
            .ReturnsAsync(true);

        var clientChecker = new Mock<IClientChecker>();
        clientChecker.Setup(checker => checker.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(true);

        await using var context = DomainContextHelper.SetupContext();

        var appointmentRepository = new AppointmentRepository(context);
        var clientRepository = new ClientRepository(context);
        var serviceRepository = new ServiceRepository(context);

        // Create a client and save it to the database
        var clientResult = await ClientBuilder.CreateValid().BuildAsync();
        await clientRepository.AddAsync(clientResult.Data);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        // Create a service and save it to the database
        var serviceResult = await Service.Create();
        await serviceRepository.AddAsync(serviceResult.Data);
        await context.SaveChangesAsync();

        var appointmentResult = await AppointmentBuilder.CreateValid()
            .WithBookedByClient(clientResult.Data.ClientId.Value)
            .WithServiceIds([serviceResult.Data.ServiceId.Value])
            .WithClientCheckerMock(clientChecker)
            .WithServiceCheckerMock(serviceChecker)
            .BuildAsync();

        var appointmentId = appointmentResult.Data.AppointmentId;

        // Act
        await appointmentRepository.AddAsync(appointmentResult.Data);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var retrievedAppointment = await appointmentRepository.GetAsync(appointmentId);

        var expectedServiceIds = appointmentResult.Data.Services.Select(s => s.ServiceId).ToList();
        var actualServiceIds = retrievedAppointment.Data.Services.Select(s => s.ServiceId).ToList();

        // Assert
        Assert.NotNull(retrievedAppointment);

        Assert.Equal(appointmentResult.Data.AppointmentId, retrievedAppointment.Data.AppointmentId);
        Assert.Equal(appointmentResult.Data.AppointmentNote, retrievedAppointment.Data.AppointmentNote);
        Assert.Equal(appointmentResult.Data.AppointmentDate, retrievedAppointment.Data.AppointmentDate);
        Assert.Equal(appointmentResult.Data.AppointmentStatus, retrievedAppointment.Data.AppointmentStatus);
        Assert.Equal(expectedServiceIds, actualServiceIds);
        Assert.Equal(appointmentResult.Data.TimeSlot, retrievedAppointment.Data.TimeSlot);
        Assert.Equal(appointmentResult.Data.BookedByClient, retrievedAppointment.Data.BookedByClient);
    }
}