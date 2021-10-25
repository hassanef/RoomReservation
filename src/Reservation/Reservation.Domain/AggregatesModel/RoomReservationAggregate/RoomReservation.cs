using Reservation.Domain.Seedwork;
using System;
using System.Collections.Generic;

namespace Reservation.Domain.AggregatesModel
{
    public class RoomReservation : Entity, IAggregateRoot
    {
        public int RoomId { get; private set; }
        public int UserId { get; private set; }
        public DateTime ReserveDate { get; private set; }
        public Period Period { get; private set; }
        private readonly List<ResourceReservation> _resourceReservations;
        public IReadOnlyCollection<ResourceReservation> ResourceReservations => _resourceReservations;

        protected RoomReservation() { }
        public RoomReservation(int roomId, int userId, Period period, Location location)
        {
            RoomId = roomId;
            UserId = userId;
            ReserveDate = DateTime.Now;
            Period = Period.Create(period.Start, period.End, location);
            _resourceReservations = new();
        }
        public void AddResourceReservation(int resourceId)
        {
            _resourceReservations.Add(new ResourceReservation(resourceId));
        }
    }
}
