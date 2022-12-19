using FluentAssertions;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.Exceptions;
using System;
using Xunit;

namespace Reservation.Domain.UnitTests.AggregateTests
{
    public class PeriodTest
    {
        [Fact]
        public void Reserved_Speciefic_Room_In_Free_Datetime()
        {
            var startDate = DateTime.Today.AddDays(1).AddHours(9);
            var endDate = DateTime.Today.AddDays(1).AddHours(11);

            var period = Period.Create(startDate, endDate, new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0));

            period.Should().NotBeNull();
        }
        [Fact]
        public void Raised_An_Exception_When_StartDate_Is_Before_Now()
        {
            var startDate = DateTime.Today.AddDays(-1).AddHours(19);
            var endDate = DateTime.Today.AddDays(-1).AddHours(20).AddMinutes(1);

            Action act = () => Period.Create(startDate, endDate, new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0));

            act.Should().Throw<RoomReservationException>()
             .WithMessage("startDate couldnt be in the past!");
        }
        [Fact]
        public void Reserved_Room_Before_Started_To_Open_Office_Raised_An_Exception()
        {
            var startDate = DateTime.Today.AddDays(1).AddHours(7).AddMinutes(59);
            var endDate = DateTime.Today.AddDays(1).AddHours(9);

            Action act = () => Period.Create(startDate, endDate, new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0));

            act.Should().Throw<RoomReservationException>()
                .WithMessage("startDate must be after Open Office time!");
        }
        [Fact]
        public void Reserved_Room_After_Closed_Office_In_Amsterdam_Raised_An_Exception()
        {
            var startDate = DateTime.Today.AddDays(1).AddHours(16);
            var endDate = DateTime.Today.AddDays(1).AddHours(17).AddMinutes(05);
            var openAmsterdamTime = new TimeSpan(08, 0, 0);
            var closeAmsterdamTime = new TimeSpan(17, 0, 0);

            Action act = () => Period.Create(startDate, endDate, openAmsterdamTime, closeAmsterdamTime);

            act.Should().Throw<RoomReservationException>()
              .WithMessage("endDate could not be greather than 17:00 in Amesterdam!");
        }
        [Fact]
        public void Reserved_Room_After_Closed_Office_In_Berlin_Raised_An_Exception()
        {
            var startDate = DateTime.Today.AddDays(1).AddHours(19);
            var endDate = DateTime.Today.AddDays(1).AddHours(20).AddMinutes(01);
            var openBerlinTime = new TimeSpan(08, 0, 0);
            var closeBerlinTime = new TimeSpan(20, 0, 0);

            Action act = () => Period.Create(startDate.AddDays(1), endDate.AddDays(1), openBerlinTime, closeBerlinTime);

            act.Should().Throw<RoomReservationException>()
             .WithMessage("endDate could not be greather than 20:00 in Berlin!");
        }
    }
}
