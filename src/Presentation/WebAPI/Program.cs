using System.Text;
using Application.Extensions;
using Application.Login.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Repositories;
using Scalar.AspNetCore;
using Services;
using Services.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Smtp"));

builder.Services.RegisterHandlers();
builder.Services.RegisterDispatcher();

builder.Services.RegisterContracts();
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();
builder.Services.RegisterUnitOfWork();
builder.Services.AddSingleton<ITokenService, TokenService>();

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

// Configure Cloud SQL connection (PostgreSQL)
var cloudSqlConnectionString = builder.Configuration.GetConnectionString("CloudSqlConnection");

builder.Services.AddSingleton<NpgsqlConnection>(_ =>
{
    var connection = new NpgsqlConnection(cloudSqlConnectionString);
    connection.Open();
    return connection;
});

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
