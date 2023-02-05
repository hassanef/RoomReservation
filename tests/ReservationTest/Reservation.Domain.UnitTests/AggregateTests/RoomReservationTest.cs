using FluentAssertions;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.UnitTests.Stubs;
using System;
using Xunit;

namespace Reservation.Domain.UnitTests.RoomReservationTests
{
    public class RoomReservationTest
    {
        private readonly StubClock _clock;
        public RoomReservationTest()
        {
            _clock = new StubClock(DateTime.Now);
            _clock.TimeTravelTo(DateTime.Parse("2020-01-05 10:30:00"));
        }
        [Fact]
        public void Created_Reservation_In_Valid_Time_Is_Not_Null()
        {
            var startDate = "2020-01-06 10:30:00";
            var endDate = "2020-01-06 11:30:00";

            var period = Period.Create(DateTime.Parse(startDate), DateTime.Parse(endDate), new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0), _clock);

            var room = new RoomReservation(1, 1, period);

            room.Should().NotBeNull();
        }
    }
}
