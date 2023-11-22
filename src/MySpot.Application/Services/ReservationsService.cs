using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public class ReservationsService : IReservationService
{
    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _repository;
    private readonly IParkingReservationService _parkingReservationService;

    public ReservationsService(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepository,
        IParkingReservationService parkingReservationService)
    {
        _clock = clock;
        _repository = weeklyParkingSpotRepository;
        _parkingReservationService = parkingReservationService;
    }
    
    public async Task<ReservationDto> GetAsync(Guid id)
    {
        var reservations = await GetAllWeeklyAsync();
        return reservations.SingleOrDefault(r => r.Id == id);
    } 

    public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
    {
        var weeklyParkingSpots = await _repository.GetAllAsync();
        return weeklyParkingSpots
            .SelectMany(x => x.Reservations)
            .Select(r => new ReservationDto()
            {
                Id = r.Id,
                ParkingSpotId = r.ParkingSpotId,
                EmployeeName = r is VehicleReservation vr ? vr.EmployeeName : string.Empty,
                Date = r.Date
            });
    }

    public async Task<Guid?> ReserveForVehicleAsync(ReserveParkingSpotForVehicle command)
    {
        var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
        var week = new Week(command.Date); //_clock.Current()

        var weeklyParkingSpots = await _repository.GetAllByWeekAsync(week);
        var weeklyParkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);

        if (weeklyParkingSpotToReserve is null)
        {
            return default;
        }
 
        var reservation = new VehicleReservation(command.ReservationId, 
            command.ParkingSpotId, command.Date, command.EmployeeName, 
            command.LicensePlate, command.Capacity);

        _parkingReservationService.ReserveSpotForVehicle(
            weeklyParkingSpots, JobTitle.Employee, 
            weeklyParkingSpotToReserve, reservation);

        await _repository.UpdateAsync(weeklyParkingSpotToReserve);

        return reservation.Id;
    }

    public async Task ReserveForCleaningAsync(ReserveParkingSpotForCleaning command)
    {
        var week = new Week(command.Date);
        
        var weeklyParkingSpots = await _repository.GetAllByWeekAsync(week);

        _parkingReservationService.ReserveParkingForCleaning(
            weeklyParkingSpots, command.Date);

        // var tasks = weeklyParkingSpots.Select(parkingSpot => _repository.UpdateAsync(parkingSpot));
        // await Task.WhenAll(tasks);

        foreach (var parkingSpot in weeklyParkingSpots)
        {
            await _repository.UpdateAsync(parkingSpot);
        }
    }


    public async Task<bool> ChangeReservationLicensePlateAsync(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations
            .OfType<VehicleReservation>()
            .SingleOrDefault(x => x.Id == command.ReservationId);
        
        if (existingReservation is null)
        {
            return false;
        }
        
        if (existingReservation.Date <= _clock.Current())
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePlate);
        await _repository.UpdateAsync(weeklyParkingSpot);

        return true;
    }

    public async Task<bool> DeleteAsync(DeleteReservation command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
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
        await _repository.DeleteAsync(weeklyParkingSpot);

        return true;
    }

    private async Task<WeeklyParkingSpot> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
    {
        var weeklyParkingSpots = await _repository.GetAllAsync();
        return weeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
    }
}