using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.AggregatesModel
{
    public record ResourceReservation
    {
        public int Id { get; init; }
        public int RoomReservationId { get; init; }
        public int ResourceId { get; init; }

        public ResourceReservation(int resourceId)
        {
            ResourceId = resourceId;
        }
    }
}
