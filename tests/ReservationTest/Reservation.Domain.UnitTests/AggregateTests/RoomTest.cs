using FluentAssertions;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.AggregatesModel.OfficeAggregate;
using Reservation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Reservation.Domain.UnitTests.AggregateTests
{
    public class RoomTest
    {
        [Fact]
        public void Create_Room_With_Speciefic_Fixed_Resource()
        {
            var room = Room.CreateRoom("Room110", 20, true, ResourceType.Fixed);

            room.Should().NotBeNull();
        }
        [Fact]
        public void Create_Room_With_Movable_Resource_Raised_An_Exception()
        {
            Action act = () => Room.CreateRoom("Room110", 20, true, ResourceType.Movable);

            act.Should().Throw<RoomReservationException>()
                .WithMessage("resource type is not valid!"); 
        }

       
        //private List<AggregatesModel.Reservation> GetRoomReservations()
        //{
        //    var reservations = new List<AggregatesModel.Reservation>();

        //    var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
        //    var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 0, 0);

        //    var reservation = new AggregatesModel.Reservation(1, Period.Create(startDate, endDate));
        //    reservations.Add(reservation);

        //    return reservations;
        //}
    }
}
