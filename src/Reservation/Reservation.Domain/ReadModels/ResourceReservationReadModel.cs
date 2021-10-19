using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.ReadModels
{
    public class ResourceReservationReadModel
    {
        public int Id { get; set; }
        public int RoomReservationId { get; set; }
        public int ResourceId { get; set; }
        public virtual RoomReservationReadModel RoomReservation{ get; set; }

    }
}
