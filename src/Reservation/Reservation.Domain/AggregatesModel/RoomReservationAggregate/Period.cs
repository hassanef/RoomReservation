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
        public static Period Create(DateTime startDate, DateTime endDate, TimeSpan openOfficeTime, TimeSpan closeOfficeTime)
        {
            if (startDate < DateTime.Now || endDate < DateTime.Now)
                throw new RoomReservationException("startDate couldnt be in the past!");
            if (startDate.TimeOfDay < openOfficeTime)
                throw new RoomReservationException("startDate must be after Open Office time!");
            if (endDate.TimeOfDay > closeOfficeTime)
                throw new RoomReservationException("endDate must be before close Office time!");

            var period = new Period(startDate, endDate);
            return period;
        }
    }
}
