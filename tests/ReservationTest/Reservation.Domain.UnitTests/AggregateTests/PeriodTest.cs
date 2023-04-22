using FluentAssertions;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.Exceptions;
using Reservation.Domain.UnitTests.Stubs;
using System;
using Xunit;

namespace Reservation.Domain.UnitTests.AggregateTests
{
    public class PeriodTest
    {
        private readonly StubClock _clock;
        public PeriodTest()
        {
            _clock = new StubClock(DateTime.Now);
            _clock.TimeTravelTo(DateTime.Parse("2020-01-05 10:30:00"));
        }
      
        [Fact]
        public void Reserve_Speciefic_DateTime_For_Room_In_Free_Datetime()
        {
            var startDate = "2020-01-06 10:30:00";
            var endDate = "2020-01-06 11:30:00";

            var period = Period.Create(DateTime.Parse(startDate), DateTime.Parse(endDate), new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0), _clock);

            period.Should().NotBeNull();
        }
        [Fact]
        public void Can_Not_Reserve_Speciefic_DateTime_For_Room_When_StartDate_Is_Before_Now()
        {
            var startDate = "2020-01-05 08:30:00";
            var endDate = "2020-01-05 09:30:00";

            Action act = () => Period.Create(DateTime.Parse(startDate), DateTime.Parse(endDate), new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0), _clock);

            act.Should().Throw<RoomReservationException>()
             .WithMessage("startDate couldnt be in the past!");
        }
        [Fact]
        public void Can_Not_Reserve_Speciefic_DateTime_For_Room_Before_Start_To_Open_Office()
        {
            var startDate = "2020-01-06 07:59:00";
            var endDate = "2020-01-06 09:00:00";

            Action act = () => Period.Create(DateTime.Parse(startDate), DateTime.Parse(endDate), new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0), _clock);

            act.Should().Throw<RoomReservationException>()
                .WithMessage("startDate must be after Open Office time!");
        }
        [Fact]
        public void Can_Not_Reserve_Speciefic_DateTime_For_Room_After_Close_Office_Time_In_Amsterdam()
        {
            var startDate = "2020-01-06 16:00:00";
            var endDate = "2020-01-06 17:05:00";
            var openAmsterdamTime = new TimeSpan(08, 0, 0);
            var closeAmsterdamTime = new TimeSpan(17, 0, 0);

            Action act = () => Period.Create(DateTime.Parse(startDate), DateTime.Parse(endDate), openAmsterdamTime, closeAmsterdamTime, _clock);

            act.Should().Throw<RoomReservationException>()
              .WithMessage("endDate must be before close Office time!");
        }
        [Fact]
        public void Can_Not_Reserved_Speciefic_DateTime_For_Room_After_Close_Office_Time_In_Berlin()
        {
            var startDate = "2020-01-06 18:00:00";
            var endDate = "2020-01-06 20:01:00";
            var openBerlinTime = new TimeSpan(08, 0, 0);
            var closeBerlinTime = new TimeSpan(20, 0, 0);

            Action act = () => Period.Create(DateTime.Parse(startDate), DateTime.Parse(endDate), openBerlinTime, closeBerlinTime, _clock);

            act.Should().Throw<RoomReservationException>()
             .WithMessage("endDate must be before close Office time!");
        }
        public void CurrentTimeIs(string datetime)
        {
            var date = DateTime.Parse(datetime);
            _clock.TimeTravelTo(date);
        }
    }
}
