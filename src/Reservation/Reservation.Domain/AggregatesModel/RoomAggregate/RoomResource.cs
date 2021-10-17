using Reservation.Domain.Seedwork;

namespace Reservation.Domain.AggregatesModel
{
    public record RoomResource 
    {
        public int Id { get; init; }
        public int RoomId { get; init; }
        public int ResourceId { get; init; }

        public RoomResource(int resourceId)
        {
            ResourceId = resourceId;
        }
    }
}
