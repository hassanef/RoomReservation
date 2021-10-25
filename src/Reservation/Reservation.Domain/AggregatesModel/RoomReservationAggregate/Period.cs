using Reservation.Domain.Exceptions;
using System;

namespace Reservation.Domain.AggregatesModel
{
    public record Period
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }

        private Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
        public static Period Create(DateTime start, DateTime end, Location location)
        {
            if (start <= DateTime.Now)
                throw new ReservationException("The start date cannot be earlier than the current date!");
            if(start.TimeOfDay < new TimeSpan(08, 0, 0))
                throw new ReservationException("The start date cannot be earlier than 08:00 AM!");
            if (location == Location.Amsterdam && end.TimeOfDay > new TimeSpan(17, 0, 0))
                throw new ReservationException("The end date cannot be later than 05:00 PM in Amsterdam!");
            if (location == Location.Berlin && end.TimeOfDay > new TimeSpan(20, 0, 0))
                throw new ReservationException("The end date cannot be later than 08:00 PM in Berlin!");

            var period = new Period(start, end);
            return period;
        }
    }
}
