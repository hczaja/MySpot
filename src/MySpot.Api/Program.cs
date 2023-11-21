using Microsoft.EntityFrameworkCore;
using MySpot.Application;
using MySpot.Application.Services;
using MySpot.Core;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllers();

var app = builder.Build();

app.UseInfrastructure();
app.MapControllers();

app.Run();
