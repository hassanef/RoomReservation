using Reservation.Domain.Exceptions;
using Reservation.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.AggregatesModel.OfficeAggregate
{
    public class Room : Entity
    {
        public int OfficeId { get; private set; }
        public string Title { get; private set; }
        public byte PersonCapacity { get; private set; }
        public bool HasChair { get; private set; }
        private readonly List<RoomResource> _roomResources;
        public IReadOnlyCollection<RoomResource> RoomResources => _roomResources;
        private Room(string title, byte personCapacity, bool hasChair)
        {
            Title = title;
            PersonCapacity = personCapacity;
            HasChair = hasChair;
            _roomResources = new();
        }
        public static Room CreateRoom(string title, byte personCapacity, bool hasChair, ResourceType resourceType)
        {
            if (resourceType == ResourceType.Movable)
                throw new RoomReservationException("resource type is not valid!");

            var room = new Room(title, personCapacity, hasChair);
            return room;
        }
      
    }
}
