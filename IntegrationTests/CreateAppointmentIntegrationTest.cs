using System.Net.Http.Json;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Values;
using DomainModelPersistence.EfcConfigs;
using EfcQueries.Models;
using IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Category = Domain.Aggregates.ServiceCategory.Category;
using Client = Domain.Aggregates.Client.Client;
using MediaUrl = Domain.Aggregates.ServiceCategory.Values.MediaUrl;
using Service = Domain.Aggregates.ServiceCategory.Entities.Service;

namespace IntegrationTests;

public class CreateAppointmentIntegrationTest : IClassFixture<GlowNestWebApplicationFactory>
{
    private readonly GlowNestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private IEmailUniqueChecker _emailUniqueChecker;

    public CreateAppointmentIntegrationTest(GlowNestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAppointment_WithValidInput_ReturnsOkAndPersistData()
    {
        Guid categoryId, serviceId;
        
        using (var scope = _factory.Services.CreateScope())
        {
            _emailUniqueChecker  = scope.ServiceProvider.GetRequiredService<IEmailUniqueChecker>();

            var writeDb = scope.ServiceProvider.GetRequiredService<DomainModelContext>();

            // create required data
            var client = await CreateClient();
            var category = await CreateCategory();
            var service = await AddServiceToCategory(category);
            
            categoryId = category.CategoryId.Value;
            serviceId = service.ServiceId.Value;
            
            // Add schedule for the booking date
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var schedule = Domain.Aggregates.Schedule.Schedule.CreateSchedule(date).Data;

            // save data to database
            writeDb.Set<Client>().Add(client);
            writeDb.Set<Category>().Add(category);
            writeDb.Set<Service>().Add(service);
            writeDb.Set<Domain.Aggregates.Schedule.Schedule>().Add(schedule);
            
            await writeDb.SaveChangesAsync();
        }
        
        var request = new
        {
            AppointmentNote = "Test appointment",
            AppointmentDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd"),
            BookedByClient = "testEmail@gmail.com",
            ServiceIds = new[] { serviceId.ToString() },
            CategoryIds = new[] { categoryId.ToString() },
            StartTime = "10:00",
            EndTime = "10:30"
        };

        var response = await _client.PostAsJsonAsync("/schedule/appointment/create", request);

        response.EnsureSuccessStatusCode();

        using var verifyScope = _factory.Services.CreateScope();
        var verifyDb = verifyScope.ServiceProvider.GetRequiredService<DomainModelContext>();
        var updatedSchedule = verifyDb.Set<Schedule>().FirstOrDefault();

        Assert.NotNull(updatedSchedule);
        Assert.Single(updatedSchedule.Appointments);
        // var appointment = updatedSchedule.Appointments.First();
        // Assert.Equal("testEmail@gmail.com", appointment.BookedByClient);
    }

    private async Task<Client> CreateClient()
    {
        var firstName = FullName.Create("Test", "User").Data;
        var email = Email.Create("testEmail@gmail.com").Data;
        var password = Password.Create("TestPassword@123").Data;
        var phoneNumber = PhoneNumber.Create("12345678").Data;

        var client = await Client.Create(firstName, email, password, phoneNumber, _emailUniqueChecker);
        return client.Data;
    }

    private async Task<Category> CreateCategory()
    {
        var categoryName = CategoryName.Create("Test Category").Data;
        var categoryDescription = CategoryDescription.Create("This is description").Data;
        var mediaUrls = new List<MediaUrl>();

        var categoryResult = await Category.Create(categoryName, categoryDescription, mediaUrls);
        return categoryResult.Data;
    }

    private async Task<Service> AddServiceToCategory(Category category)
    {
        var addedServiceResult = await category.AddService(ServiceName.Create("Test Service").Data,
            Price.Create(100).Data, TimeSpan.FromMinutes(30));
        return addedServiceResult.Data;
    }
    
}