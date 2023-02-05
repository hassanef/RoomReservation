using System;

namespace Reservation.Domain.Utils
{
    public class SystemClock : IClock
    {
        public DateTime Now() => DateTime.Now;
    }
}
