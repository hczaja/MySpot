using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public sealed class ReserveParkingSpotForVehicleHandler
    : ICommandHandler<ReserveParkingSpotForVehicle>
{
    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IParkingReservationService _parkingReservationService;

    public ReserveParkingSpotForVehicleHandler(IClock clock, 
        IWeeklyParkingSpotRepository weeklyParkingSpotRepository,
        IParkingReservationService parkingReservationService)
    {
        _clock = clock;
        _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
        _parkingReservationService = parkingReservationService;
    }

    public async Task HandleAsync(ReserveParkingSpotForVehicle command)
    {
        var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
        var week = new Week(_clock.Current());

        var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetAllByWeekAsync(week);
        var weeklyParkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);

        if (weeklyParkingSpotToReserve is null)
            throw new WeeklyParkingSpotNotFoundException(parkingSpotId);
 
        var reservation = new VehicleReservation(command.ReservationId, 
            command.ParkingSpotId, command.Date, command.EmployeeName, 
            command.LicensePlate, command.Capacity);

        _parkingReservationService.ReserveSpotForVehicle(
            weeklyParkingSpots, JobTitle.Employee, 
            weeklyParkingSpotToReserve, reservation);

        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpotToReserve);
    }
}
