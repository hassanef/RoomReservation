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
    public class ResourceReservationCommandHandler : IRequestHandler<ResourceReservationCommand, bool>
    {
        private readonly IRoomReservationRepository _repository;

        public ResourceReservationCommandHandler(IRoomReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ResourceReservationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ReservationException("request CreateResourceReservationCommandHandler is null!");

            var roomReservation = await _repository.SingleOrDefaultAsync(x => x.Id == request.RoomReservationId);

            if(roomReservation == null)
                throw new ReservationException("request RoomReservation is null!");

            roomReservation.AddResourceReservation(request.ResourceId);

            await _repository.CreateAsync(roomReservation);

            return true;
        }
    }
}