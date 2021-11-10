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
        public static Period Create(DateTime start, DateTime end)
        {
            var period = new Period(start, end);
            return period;
        }
    }
}
