using MediatR;
using Reservation.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Application.Commands
{
    public class CreateRoomReservationCommand : IRequest<bool>
    {
        public int RoomId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int LocationId { get; init; }
    }
}
