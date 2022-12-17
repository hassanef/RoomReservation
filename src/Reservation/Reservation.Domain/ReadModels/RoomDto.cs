using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.ReadModels
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int OfficeId { get;  set; }
        public string Title { get; set; }
        public byte PersonCapacity { get; set; }
        public bool HasChair { get; set; }
        public virtual OfficeDto Office { get; set; }
        public virtual ICollection<RoomReservationDto> RoomReservations { get; set; }

    }
}
