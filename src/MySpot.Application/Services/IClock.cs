using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public interface IClock
{
    Date Current();
}
