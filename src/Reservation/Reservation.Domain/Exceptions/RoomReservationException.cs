using System;

namespace Reservation.Domain.Exceptions
{
    public class RoomReservationException : Exception
    {
        public RoomReservationException()
        { }

        public RoomReservationException(string message)
            : base(message)
        { }

        public RoomReservationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

