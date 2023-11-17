using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;
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
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "Jon Snow", "ABC123", new Date(invalidDate));

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
        
        var reservation = new Reservation(
            Guid.NewGuid(), _weeklyParkingSpot.Id, "Jon Snow", "ABC123", reservationDate);
        _weeklyParkingSpot.AddReservation(reservation, _date);

        var nextReservation = new Reservation(
            Guid.NewGuid(), _weeklyParkingSpot.Id, "Jon Snow", "ABC123", reservationDate);
    
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

        var reservation = new Reservation(
            Guid.NewGuid(), _weeklyParkingSpot.Id, "Jon Snow", "ABC123", reservationDate);
        _weeklyParkingSpot.AddReservation(reservation, _date);

        _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
    }

    #region  Arrange

    private readonly Date _date;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;

    public WeeklyParkingSpotTests()
    {
        _date = new Date(new DateTime(2023, 11, 18));
        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_date), "P1");
    }

    #endregion
}