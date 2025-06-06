/*using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.Appointment;
using Domain.Aggregates.Schedule.Values.AppointmentValues;
using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Contracts;
using DomainModelPersistence.ClientPersistence;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories;
using IntegrationTests.Helpers;
using Moq;
using UnitTest.Features.Helpers.Builders;

namespace IntegrationTests.RepositoriesTest;

public class ScheduleRepoTest
{
    [Fact]
    public async Task SaveAndRetrieveScheduleWithAppointment_ShouldMatchOriginal()
    {
        // Arrange
        var (context, scheduleRepository, clientRepository, categoryrepo) = SetupRepositories();

        var serviceChecker = SetupServiceCheckerMock();
        var clientChecker = SetupClientCheckerMock();
        var dateTimeProvider = SetupDateTimeProviderMock();

        var client = await PersistClientAsync(clientRepository, context);
        var service = await PersistServiceAsync(categoryrepo, context);

        var appointmentDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
        var appointmentDto = CreateValidAppointmentDto(service.ServiceId, client.ClientId, appointmentDate);

        var schedule = Schedule.CreateSchedule(appointmentDate).Data;
        await scheduleRepository.AddAsync(schedule);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var trackedSchedule = (await scheduleRepository.GetScheduleByDateAsync(appointmentDate)).Data;

        // Act
        var addResult = await trackedSchedule.AddAppointment(appointmentDto, serviceChecker.Object,
            clientChecker.Object, dateTimeProvider.Object);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var retrievedSchedule = await scheduleRepository.GetScheduleByDateAsync(appointmentDate);

        // Assert
        Assert.True(retrievedSchedule.IsSuccess);
        Assert.Single(retrievedSchedule.Data.Appointments);

        var expected = addResult.Data.Appointments[0];
        var actual = retrievedSchedule.Data.Appointments[0];

        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.AppointmentNote, actual.AppointmentNote);
    }

    private static (DomainModelContext context, ScheduleRepository scheduleRepo, ClientRepository clientRepo, CategoryRepository categoryrepo) SetupRepositories()
    {
        var context = DomainContextHelper.SetupContext();
        return (
            context,
            new ScheduleRepository(context),
            new ClientRepository(context),
            new CategoryRepository(context)
        );
    }

    private static Mock<IServiceChecker> SetupServiceCheckerMock()
    {
        var mock = new Mock<IServiceChecker>();
        mock.Setup(x => x.DoesServiceExistsAsync(It.IsAny<ServiceId>())).ReturnsAsync(true);
        return mock;
    }

    private static Mock<IClientChecker> SetupClientCheckerMock()
    {
        var mock = new Mock<IClientChecker>();
        mock.Setup(x => x.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(true);
        return mock;
    }

    private static Mock<IDateTimeProvider> SetupDateTimeProviderMock()
    {
        var mock = new Mock<IDateTimeProvider>();
        mock.Setup(x => x.GetNow()).Returns(DateTime.Now);
        return mock;
    }

    private static async Task<Client> PersistClientAsync(ClientRepository repo, DomainModelContext context)
    {
        var clientResult = await ClientBuilder.CreateValid().BuildAsync();
        await repo.AddAsync(clientResult.Data);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        return clientResult.Data;
    }

    private static async Task<Service> PersistServiceAsync(CategoryRepository repo, DomainModelContext context)
    {
        var serviceName = Name.Create("Valid Service").Data;
        var serviceDescription = Description.Create("Valid Service Description").Data;
        var servicePrice = Price.Create(100).Data;
        var serviceDuration = TimeSpan.FromHours(1);
        var serviceMediaUrls = new List<MediaUrl>
        {
            MediaUrl.Create("http://example.com/image1.jpg").Data,
            MediaUrl.Create("http://example.com/image2.jpg").Data
        };
        var serviceResult = await Service.Create(serviceName, serviceDescription, servicePrice, serviceDuration, serviceMediaUrls);
        await repo.AddAsync(serviceResult.Data);
        await context.SaveChangesAsync();
        return serviceResult.Data;
    }

    private static CreateAppointmentDto
        CreateValidAppointmentDto(ServiceId serviceId, ClientId clientId, DateOnly date) =>
        new(
            AppointmentNote.Create("Valid appointment note").Data,
            TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")).Data,
            date,
            [serviceId],
            clientId
        );
}*/