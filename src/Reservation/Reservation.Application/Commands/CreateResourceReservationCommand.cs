using MediatR;
using Reservation.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Application.Commands
{
    public class CreateResourceReservationCommand : IRequest<bool>
    {
        public int RoomReservationId { get; init; }
        public int ResourceId { get; init; }
        public ResourceType ResourceType { get; set; }
    }
}
