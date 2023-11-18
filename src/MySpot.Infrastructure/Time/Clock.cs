using MySpot.Application.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.Services;

public class Clock : IClock
{
    public Date Current() => new(DateTimeOffset.Now);    
}