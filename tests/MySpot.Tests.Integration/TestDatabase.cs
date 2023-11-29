using Microsoft.EntityFrameworkCore;
using MySpot.Infrastructure;
using MySpot.Infrastructure.DAL;

namespace MySpot.Tests.Integration;

internal class TestDatabase : IDisposable
{
    public MySpotDbContext DbContext { get; }
    
    public TestDatabase()
    {
        var options = new OptionsProvider().Get<PostgresOptions>("postgres");
        DbContext = new MySpotDbContext(
            new DbContextOptionsBuilder<MySpotDbContext>()
                .UseNpgsql(options.ConnectionString).Options);
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}
