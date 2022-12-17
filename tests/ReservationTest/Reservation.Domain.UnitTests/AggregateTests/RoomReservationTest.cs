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
        public void Reserved_Speciefic_Room_In_Free_Datetime()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0);

            var roomReservation = RoomReservation.Create(1, 1, startDate.AddDays(1), endDate.AddDays(1), new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0));

            roomReservation.Should().NotBeNull();
        }
        [Fact]
        public void Raised_An_Exception_When_StartDate_Is_Before_Now()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 01, 0);

            Action act = () => RoomReservation.Create(1, 1, startDate.AddDays(-1), endDate, new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0));

            act.Should().Throw<RoomReservationException>()
             .WithMessage("startDate couldnt be in the past!");
        }
        [Fact]
        public void Reserved_Room_Before_Started_To_Open_Office_Raised_An_Exception()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 59, 0);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);

            Action act = () => RoomReservation.Create(1, 1, startDate.AddDays(1), endDate.AddDays(1), new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0));

            act.Should().Throw<RoomReservationException>()
                .WithMessage("startDate must be after 08:00 oclock!");
        }
        [Fact]
        public void Reserved_Room_After_Closed_Office_In_Amsterdam_Raised_An_Exception()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 05, 0);
            var openAmsterdamTime = new TimeSpan(08, 0, 0);
            var closeAmsterdamTime = new TimeSpan(17, 0, 0);

            Action act = () => RoomReservation.Create(1, 1, startDate.AddDays(1), endDate.AddDays(1), openAmsterdamTime, closeAmsterdamTime);

            act.Should().Throw<RoomReservationException>()
              .WithMessage("endDate could not be greather than 17:00 in Amesterdam!");
        }
        [Fact]
        public void Reserved_Room_After_Closed_Office_In_Berlin_Raised_An_Exception()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 01, 0);
            var openBerlinTime = new TimeSpan(08, 0, 0);
            var closeBerlinTime = new TimeSpan(20, 0, 0);

            Action act = () => RoomReservation.Create(1, 1, startDate.AddDays(1), endDate.AddDays(1), openBerlinTime, closeBerlinTime);

            act.Should().Throw<RoomReservationException>()
             .WithMessage("endDate could not be greather than 20:00 in Berlin!");
        }
    }
}
