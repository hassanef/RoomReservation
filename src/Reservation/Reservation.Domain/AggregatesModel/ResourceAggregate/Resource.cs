using Reservation.Domain.Seedwork;
using System.Collections.Generic;

namespace Reservation.Domain.AggregatesModel
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
