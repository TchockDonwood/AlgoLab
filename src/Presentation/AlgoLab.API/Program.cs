using AlgoLab.Infrastructure;
using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(
    provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddControllers();

//Регистрация алгоритмов
builder.Services.AddAlgorithms();

//Подключение бенчмарка
builder.Services.AddBenchmarking();

var app = builder.Build();

app.MapControllers();

app.Run();