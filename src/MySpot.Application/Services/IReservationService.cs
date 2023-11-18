using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.Services;

public interface IReservationService
{
    ReservationDto Get(Guid id);
    IEnumerable<ReservationDto> GetAllWeekly();
    Guid? Create(CreateReservation command);
    bool Update(ChangeReservationLicensePlate command);
    bool Delete(DeleteReservation command);
}