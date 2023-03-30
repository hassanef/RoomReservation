using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.AggregatesModel.OfficeAggregate
{
    public record RoomResource
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ResourceId { get; set; }
        public RoomResource(int resourceId)
        {
            ResourceId = resourceId;
        }
    }
}
