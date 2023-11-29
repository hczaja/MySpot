using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MySpot.Tests.Integration")]
namespace MySpot.Infrastructure;

internal sealed class PostgresOptions
{
    public string ConnectionString { get; set; }
}