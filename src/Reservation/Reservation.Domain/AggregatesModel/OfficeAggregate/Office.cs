using Reservation.Domain.Seedwork;
using System.Collections.Generic;

namespace Reservation.Domain.AggregatesModel
{
    public class Office:Entity,IAggregateRoot
    {
        public string Title { get; private set; }
        public Location Location { get; private set; }

        private readonly List<Room> _rooms;
        public IReadOnlyCollection<Room> Rooms => _rooms;

        public Office(string title, Location location)
        {
            Title = title;
            Location = location;
            _rooms = new();
        }
        public void AddOffice(string title, byte personCapacity, bool hasChair)
        {
            _rooms.Add(new Room(title, personCapacity, hasChair));
        }
    }
}
