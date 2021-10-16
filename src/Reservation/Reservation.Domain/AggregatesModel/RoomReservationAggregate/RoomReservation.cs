using Reservation.Domain.AggregatesModel.RoomAggregate;
using Reservation.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.AggregatesModel.RoomReservationAggregate
{
    public class RoomReservation : Entity, IAggregateRoot
    {
        public int RoomId { get; private set; }
        public int UserId { get; private set; }
        public DateTime ReserveDate { get; private set; }
        public Period Period { get; init; }

        public RoomReservation(int roomId, int userId, Period period, Location location)
        {
            RoomId = roomId;
            UserId = userId;
            ReserveDate = DateTime.Now;
            Period = Period.Create(period.Start, period.End, location);
        }
    }
}
