using System.Text;
using Application.Extensions;
using Domain.Common.OperationResult;
using DomainModelPersistence;
using DomainModelPersistence.EfcConfigs;
using EfcQueries.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using Scalar.AspNetCore;
using Services;
using Services.Authentication;
using Services.Email;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Smtp"));

builder.Services.RegisterHandlers();
builder.Services.RegisterDispatcher();
builder.Services.AddScoped<IQueryHandler<LoginUserQuery, Result<LoginUserResponse>>, LoginUserQueryHandler>();
builder.Services.AddScoped<IQueryDispatcher>(provider => new QueryDispatcher(provider));

builder.Services.RegisterContracts();
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();
builder.Services.RegisterUnitOfWork();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:SecretKey"]!)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddScoped<DomainModelContext>();

builder.Configuration.GetConnectionString("CloudSqlConnection");


string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DomainModelContext>(options =>
    options.UseNpgsql(connectionString!, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null);
    }));

var app = builder.Build();

app.MapControllers();

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
