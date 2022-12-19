using Reservation.Domain.Exceptions;
using Reservation.Domain.Seedwork;
using System;
using System.Collections.Generic;

namespace Reservation.Domain.AggregatesModel
{
    public class RoomReservation :Entity, IAggregateRoot
    {
        public int RoomId { get; private set; }
        public int UserId { get; private set; }
        public DateTime ReserveDate { get; private set; }
        public Period Period { get; private set; }
        private readonly List<ResourceReservation> _resourceReservations;
        public IReadOnlyCollection<ResourceReservation> ResourceReservations => _resourceReservations;

        protected RoomReservation() { }
        public RoomReservation(int userId, int roomId, DateTime startDate, DateTime endDate, TimeSpan openOfficeTime, TimeSpan closeOfficeTime)
        {
            UserId = userId;
            RoomId = roomId;
            ReserveDate = DateTime.Now;
            Period = Period.Create(startDate, endDate, openOfficeTime, closeOfficeTime);
            _resourceReservations = new();
        }
        public void AddResourceReservation(int resourceId, ResourceType resourceType)
        {
            _resourceReservations.Add(ResourceReservation.Create(resourceId, resourceType));
        }
      
    }
}
