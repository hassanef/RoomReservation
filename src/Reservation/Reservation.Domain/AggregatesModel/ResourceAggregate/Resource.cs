using Reservation.Domain.AggregatesModel.RoomAggregate;
using Reservation.Domain.Seedwork;

namespace Reservation.Domain.AggregatesModel.ResourceAggregate
{
    public class Resource: Entity, IAggregateRoot
    {
        public string Title { get; private set; }
        public ResourceType Type { get; private set; }

        public Resource(string title, ResourceType type)
        {
            Title = title;
            Type = type;
        }
    }
}
