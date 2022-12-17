using Reservation.Domain.Seedwork;
using System;
using System.Collections.Generic;

namespace Reservation.Domain.ReadModels
{
    public class RoomReservationDto 
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime ReserveDate { get; set; }
        public DateTime Period_Start{ get; set; }
        public DateTime Period_End { get; set; }
        public virtual ICollection<ResourceReservationDto> ResourceReservations { get; set; }
        public virtual RoomDto Room { get; set; }
    }
}
