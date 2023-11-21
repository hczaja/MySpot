using MySpot.Core.ValueObjects;

namespace MySpot.Core.Services;

public interface IClock
{
    Date Current();
}
