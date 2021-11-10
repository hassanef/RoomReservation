using MediatR;
using Reservation.Application.Commands;
using Reservation.Domain.Exceptions;
using Reservation.Domain.IRepositories;
using System.Threading;
using System.Threading.Tasks;

namespace Reservation.Application.CommandHandlers
{
    public class CreateResourceReservationCommandHandler : IRequestHandler<CreateResourceReservationCommand, bool>
    {
        private readonly IRoomReservationRepository _repository;

        public CreateResourceReservationCommandHandler(IRoomReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CreateResourceReservationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ReservationException("request CreateResourceReservationCommandHandler is null!");

            var roomReservation = await _repository.SingleOrDefaultAsync(x => x.Id == request.RoomReservationId);

            if(roomReservation == null)
                throw new ReservationException("request RoomReservation is null!");

            roomReservation.AddResourceReservation(request.ResourceId);

            await _repository.UpdateAsync(roomReservation);

            return true;
        }
    }
}