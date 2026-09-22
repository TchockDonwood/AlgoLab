using AlgoLab.Infrastructure;
using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AlgoLab.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(
    provider => provider.GetRequiredService<AppDbContext>());

// Слои
builder.Services.AddApplication();      // handler'ы
builder.Services.AddBenchmarking();     // из DependencyInjection.cs в Infrastructure
builder.Services.AddAlgorithms();       // реализации алгоритмов + registry

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy( policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// MVC
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();