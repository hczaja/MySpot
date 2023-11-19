using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Infrastructure.DAL.Configurations;

namespace MySpot.Infrastructure.DAL;

public sealed class MySpotDbContext : DbContext
{
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<WeeklyParkingSpot> WeeklyParkingSpots { get; set; }

    public MySpotDbContext(DbContextOptions<MySpotDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}