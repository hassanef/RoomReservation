using System;

namespace Reservation.Domain.Utils
{
    public interface IClock
    {
        DateTime Now();
    }
}
