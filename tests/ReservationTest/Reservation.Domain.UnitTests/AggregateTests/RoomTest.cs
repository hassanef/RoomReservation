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
    }
}
