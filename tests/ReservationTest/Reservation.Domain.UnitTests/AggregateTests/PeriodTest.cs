using FluentAssertions;
using Reservation.Domain.AggregatesModel.RoomAggregate;
using Reservation.Domain.AggregatesModel.RoomReservationAggregate;
using Reservation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Reservation.Domain.UnitTests.AggregateTests
{
    public class PeriodTest
    {
        [Fact]
        public void Create_RoomReservaion_With_Valid_DateTime_For_Amsterdam()
        {
            var period = Period.Create(GetValidStartDateTime(), GetValidEndDateTime(), Location.Amsterdam);

            var roomReservation = new RoomReservation(1, 1, period, Location.Amsterdam);

            roomReservation.Should().NotBeNull();
        }
        [Fact]
        public void Create_RoomReservaion_With_Valid_DateTime_For_Berlin()
        {
            var period = Period.Create(GetValidStartDateTime(), GetValidEndDateTime_ForBerlin(), Location.Berlin);

            var roomReservation = new RoomReservation(1, 1, period, Location.Berlin);

            roomReservation.Should().NotBeNull();
        }
 
        private DateTime GetValidStartDateTime()
        {
            DateTime nowDate = DateTime.Now;
            DateTime startDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                08, 01, 0);

            return startDate;
        }
        private DateTime GetValidEndDateTime()
        {
            DateTime nowDate = DateTime.Now;
            DateTime endDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                16, 59, 0);

            return endDate;
        }
        private DateTime GetValidEndDateTime_ForBerlin()
        {
            DateTime nowDate = DateTime.Now;
            DateTime endDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                19, 59, 0);

            return endDate;
        }
    }
}
