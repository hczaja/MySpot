using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;

namespace MySpot.Application.Services;

public class ReservationsService : IReservationService
{
    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _repository;

    public ReservationsService(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
    {
        _clock = clock;
        _repository = weeklyParkingSpotRepository;
    }
    
    public ReservationDto Get(Guid id) => GetAllWeekly().SingleOrDefault(r => r.Id == id);

    public IEnumerable<ReservationDto> GetAllWeekly() => 
        _repository
            .GetAll()
            .SelectMany(x => x.Reservations)
            .Select(r => new ReservationDto()
            {
                Id = r.Id,
                ParkingSpotId = r.ParkingSpotId,
                EmployeeName = r.EmployeeName,
                Date = r.Date
            });

    public Guid? Create(CreateReservation command)
    {
        var weeklyParkingSpot = _repository.Get(command.ParkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }
 
        var reservation = new Reservation(command.ReservationId, 
            command.ParkingSpotId, command.EmployeeName,
            command.LicensePlate, command.Date);

        weeklyParkingSpot.AddReservation(reservation, _clock.Current());
        _repository.Update(weeklyParkingSpot);

        return reservation.Id;
    }

    public bool Update(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }
        
        if (existingReservation.Date <= _clock.Current())
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePlate);
        _repository.Update(weeklyParkingSpot);

        return true;
    }

    public bool Delete(DeleteReservation command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(r => r.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }

        weeklyParkingSpot.RemoveReservation(command.ReservationId);
        _repository.Delete(weeklyParkingSpot);

        return true;
    }

    private WeeklyParkingSpot GetWeeklyParkingSpotByReservation(Guid reservationId)
        => _repository.GetAll().SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
}