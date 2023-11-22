using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Policies;
using MySpot.Core.Services;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Services;
using MySpot.Tests.Unit.Shared;
using Shouldly;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    [Test]
    [TestCase("2023-11-17")]
    [TestCase("2023-11-26")]
    public void given_invalid_date_add_reservation_should_fail(string dateTime)
    {
        // arrange
        var invalidDate = DateTime.Parse(dateTime);
        var reservation = new VehicleReservation(Guid.NewGuid(), _weeklyParkingSpot.Id, new Date(invalidDate), "Jon Snow", "ABC123");

        // act
        var exception = Assert.Catch(
            typeof(InvalidReservationDayException), 
            () => _weeklyParkingSpot.AddReservation(reservation, _date));

        // assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDayException>();
    }

    [Test]
    public void given_reservation_for_already_existing_date_add_reservation_should_fail()
    {
        // arrange
        var reservationDate = _date.AddDays(1);
        
        var reservation = new VehicleReservation(
            Guid.NewGuid(), _weeklyParkingSpot.Id, reservationDate, "Jon Snow", "ABC123");
        _weeklyParkingSpot.AddReservation(reservation, _date);

        var nextReservation = new VehicleReservation(
            Guid.NewGuid(), _weeklyParkingSpot.Id, reservationDate, "Jon Snow", "ABC123");
    
        // act
        var exception = Assert.Catch(
            typeof(ParkingSpotAlreadyReservedException), 
            () => _weeklyParkingSpot.AddReservation(nextReservation, reservationDate));

        // assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }

    [Test]
    public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
    {
        var reservationDate = _date.AddDays(1);

        var reservation = new VehicleReservation(
            Guid.NewGuid(), _weeklyParkingSpot.Id, reservationDate, "Jon Snow", "ABC123");
        _weeklyParkingSpot2.AddReservation(reservation, _date);

        _weeklyParkingSpot2.Reservations.ShouldHaveSingleItem();
    }

    #region  Arrange

    private readonly Date _date;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;
    private readonly WeeklyParkingSpot _weeklyParkingSpot2;

    public WeeklyParkingSpotTests()
    {
        _date = new Date(new DateTime(2023, 11, 18));

        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_date), "P1");
        _weeklyParkingSpot2 = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_date), "P2");
    }

    #endregion
}
