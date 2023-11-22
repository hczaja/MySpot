using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.Services;

public class Clock : IClock
{
    public Date Current() => new(DateTime.UtcNow);    
}