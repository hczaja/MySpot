using MySpot.Api.Entities;
using MySpot.Api.Repositories;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<IClock, Clock>()
    .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
    .AddSingleton<IReservationService, ReservationsService>()
    .AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
