using Reservation.Domain.Seedwork;
using System;

namespace Reservation.Domain.AggregatesModel
{
    public class Location :Entity, IAggregateRoot
    {
        public string Title { get; private set; }
        public TimeSpan Start { get; private set; }
        public TimeSpan End { get; private set; }
        public Location(int id, string title, TimeSpan start, TimeSpan end)
        {
            Id = id;
            Title = title;
            Start = start;
            End = end;
        }
    }
}
