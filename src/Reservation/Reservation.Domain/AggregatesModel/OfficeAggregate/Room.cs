using Reservation.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.AggregatesModel
{
    public class Room: Entity
    {
        public int OfficeId { get; private set; }
        public string Title { get; private set; }
        public byte PersonCapacity { get; private set; }
        public bool HasChair { get; private set; }
        private readonly List<RoomResource> _roomResources;
        public IReadOnlyCollection<RoomResource> RoomResources => _roomResources;
        public Room(string title, byte personCapacity, bool hasChair)
        {
            Title = title;
            PersonCapacity = personCapacity;
            HasChair = hasChair;
            _roomResources = new();
        }
    }
}
