using Reservation.Domain.Exceptions;

namespace Reservation.Domain.AggregatesModel
{
    public record ResourceReservation
    {
        public int Id { get; init; }
        public int RoomReservationId { get; init; }
        public int ResourceId { get; init; }
        protected ResourceReservation() { }
        private ResourceReservation(int resourceId)
        {
            ResourceId = resourceId;
        }
        public static ResourceReservation Create(int resourceId, ResourceType resourceType)
        {
            if (resourceType == ResourceType.Fixed)
                throw new RoomReservationException("fix resource can not be move!");

            var resourceReservation = new ResourceReservation(resourceId);
            return resourceReservation;
        }
    }
}
