using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Application.Commands
{
    public class DeleteRoomReservationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
