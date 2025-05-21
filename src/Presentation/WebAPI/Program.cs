using System.Security.Claims;
using System.Text;
using Application.Extensions;
using DomainModelPersistence;
using EfcQueries.Extension;
using EfcQueries.Models;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Services;
using Services.Email;
using Services.Jobs;
using Services.Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    // add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
    options.CustomSchemaIds(type => type.FullName);
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Smtp"));

builder.Services.RegisterApplications();
builder.Services.RegisterDmPersistence();

builder.Services.RegisterQueryHandlers();

builder.Services.RegisterServices();
builder.Services.RegisterDatabase(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:SecretKey"]!)),
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidAudience = "GlowNestAPI",
            ValidateIssuer = true,
            ValidIssuer = "GlowNestServer",
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role,
        };
    });


builder.Configuration.GetConnectionString("CloudSqlConnection");

builder.Services.AddHangfire(configuration =>
    configuration.UsePostgreSqlStorage(config =>
        config.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOptions<StripeSettings>()
    .Bind(builder.Configuration.GetSection("Stripe"))
    .ValidateDataAnnotations();

builder.Services.AddHangfireServer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobs.AddOrUpdate<ScheduleSeederJob>(
        "SeedFutureSchedules",
        job => job.SeedFutureSchedulesAsync(),
        Cron.Daily(0));
}

app.MapControllers();
app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapScalarApiReference();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

app.Run();

public partial class Program { }
