using Reservation.Domain.AggregatesModel.OfficeAggregate;
using Reservation.Domain.Seedwork;
using System.Collections.Generic;

namespace Reservation.Domain.AggregatesModel
{
    public class Office:Entity,IAggregateRoot
    {
        public string Title { get; private set; }
        public int LocationId { get; private set; }

        private readonly List<Room> _rooms;
        public IReadOnlyCollection<Room> Rooms => _rooms;

        public Office(string title, int locationId)
        {
            Title = title;
            LocationId = locationId;
            _rooms = new();
        }
        public void AddRoom(string title, byte capacity, bool hasChair, ResourceType resourceType)
        {
            _rooms.Add(Room.CreateRoom(title, capacity, hasChair, resourceType));
        }
    }
}
