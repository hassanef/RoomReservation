using FluentAssertions;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.AggregatesModel.OfficeAggregate;
using Reservation.Domain.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace Reservation.Domain.UnitTests.RoomReservationTests
{
    public class RoomReservationTest
    {
        [Fact]
        public void Created_Reservation_In_Valid_Time_Is_Not_Null()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0);
            var period = Period.Create(startDate.AddDays(1), endDate.AddDays(1), new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0));

            var room = new RoomReservation(1, 1, period);

            room.Should().NotBeNull();
        }
    }
}
