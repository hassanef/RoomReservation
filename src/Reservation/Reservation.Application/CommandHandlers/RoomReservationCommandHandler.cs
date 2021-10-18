using MediatR;
using Reservation.Application.Commands;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.Exceptions;
using Reservation.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reservation.Application.CommandHandlers
{
    public class RoomReservationCommandHandler : IRequestHandler<RoomReservationCommand, bool>
    {
        private readonly IRoomReservationRepository _repository;

        public RoomReservationCommandHandler(IRoomReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RoomReservationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ReservationException("request CreateRoomReservationCommand is null!");

            var period = Period.Create(request.StartDate, request.EndDate, request.Location);
            var roomReservation = new RoomReservation(request.RoomId, request.UserId, period, request.Location);

            await _repository.CreateAsync(roomReservation);

            return true;
        }
    }
}