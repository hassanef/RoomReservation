using Reservation.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.AggregatesModel.RoomAggregate
{
    public class RoomResource: Entity
    {
        public int ResourceId { get; private set; }
        public int Count { get; private set; }

        public RoomResource(int resourceId, int count)
        {
            ResourceId = resourceId;
            Count = count;
        }
    }
}
