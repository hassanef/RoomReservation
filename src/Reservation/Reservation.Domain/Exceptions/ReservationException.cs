using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.Exceptions
{
    public class ReservationException : Exception
    {
        public ReservationException()
        { }

        public ReservationException(string message)
            : base(message)
        { }

        public ReservationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

