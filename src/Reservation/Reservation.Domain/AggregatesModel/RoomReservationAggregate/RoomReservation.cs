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
        private RoomReservation(int userId, int roomId, DateTime startDate, DateTime endDate)
        {
            UserId = userId;
            RoomId = roomId;
            ReserveDate = DateTime.Now;
            Period = Period.Create(startDate, endDate);
            _resourceReservations = new();
        }
        public static RoomReservation Create(int userId, int roomId, DateTime startDate, DateTime endDate, TimeSpan openOfficeTime, TimeSpan CloseOfficeTime)
        {
            if (startDate < DateTime.Now || endDate < DateTime.Now)
                throw new RoomReservationException("startDate couldnt be in the past!");
            if (startDate.TimeOfDay < new TimeSpan(08, 0, 0))
                throw new RoomReservationException("startDate must be after 08:00 oclock!");
            if (endDate.TimeOfDay > new TimeSpan(17, 0, 0) && CloseOfficeTime <= new TimeSpan(17, 0, 0))
                throw new RoomReservationException("endDate could not be greather than 17:00 in Amesterdam!");
            if (endDate.TimeOfDay > new TimeSpan(17, 0, 0) && CloseOfficeTime <= new TimeSpan(20, 0, 0))
                throw new RoomReservationException("endDate could not be greather than 20:00 in Berlin!");
          
            var roomReservation = new RoomReservation(userId, roomId, startDate, endDate);
            return roomReservation;
        }
        public void AddResourceReservation(int resourceId, ResourceType resourceType)
        {
            _resourceReservations.Add(ResourceReservation.Create(resourceId, resourceType));
        }
      
    }
}
