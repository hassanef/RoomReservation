using FluentAssertions;
using Reservation.Domain.AggregatesModel.RoomAggregate;
using Reservation.Domain.AggregatesModel.RoomReservationAggregate;
using Reservation.Domain.Exceptions;
using System;
using Xunit;

namespace Reservation.Domain.UnitTests.AggregateTests
{
    public class PeriodTest
    {
        [Fact]
        public void Create_Period_With_Valid_DateTime_For_Amsterdam()
        {
            var period = Period.Create(GetValidStartDateTime(), GetValidEndDateTime(), Location.Amsterdam);

            var roomReservation = new RoomReservation(1, 1, period, Location.Amsterdam);

            roomReservation.Should().NotBeNull();
        }
        [Fact]
        public void Create_Period_With_Valid_DateTime_For_Berlin()
        {
            var period = Period.Create(GetValidStartDateTime(), GetValidEndDateTime_ForBerlin(), Location.Berlin);

            period.Should().NotBeNull();
        }
        [Fact]
        public void Create_Period_With_InValid_StartDateTime_For_Amsterdam()
        {
            Action act = () => { var period = Period.Create(GetInValidStartDateTime(), GetInValidEndDateTime_ForAmsterdam(), Location.Amsterdam); };

            act.Should().Throw<ReservationException>();
        }
        [Fact]
        public void Create_Period_With_InValid_StartDateTime_For_Berlin()
        {
            Action act = () => { var period = Period.Create(GetInValidStartDateTime(), GetInValidEndDateTime_ForBerlin(), Location.Berlin); };

            act.Should().Throw<ReservationException>();
        }
        [Fact]
        public void Create_Period_With_InValid_EndDateTime_For_Berlin()
        {
            Action act = () => { var period = Period.Create(GetValidStartDateTime(), GetInValidEndDateTime_ForBerlin(), Location.Berlin); };

            act.Should().Throw<ReservationException>();
        }
        [Fact]
        public void Create_Period_With_InValid_EndDateTime_For_Amsterdam()
        {
            Action act = () => { var period = Period.Create(GetValidStartDateTime(), GetInValidEndDateTime_ForAmsterdam(), Location.Amsterdam); };

            act.Should().Throw<ReservationException>();
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
        private DateTime GetInValidStartDateTime()
        {
            DateTime nowDate = DateTime.Now;
            DateTime startDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                07, 59, 0);

            return startDate;
        }
        private DateTime GetInValidEndDateTime_ForAmsterdam()
        {
            DateTime nowDate = DateTime.Now;
            DateTime endDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                17, 01, 0);

            return endDate;
        }
        private DateTime GetInValidEndDateTime_ForBerlin()
        {
            DateTime nowDate = DateTime.Now;
            DateTime endDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                20, 01, 0);

            return endDate;
        }
    }
}
