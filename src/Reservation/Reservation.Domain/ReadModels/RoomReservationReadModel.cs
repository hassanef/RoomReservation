using Reservation.Domain.Seedwork;
using System;
using System.Collections.Generic;

namespace Reservation.Domain.ReadModels
{
    public class RoomReservationReadModel 
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime ReserveDate { get; set; }
        public DateTime Period_Start{ get; set; }
        public DateTime Period_End { get; set; }
        public virtual ICollection<ResourceReservationReadModel> ResourceReservations { get; set; }
        public virtual RoomReadModel Room { get; set; }
    }
}
