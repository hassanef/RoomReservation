using MediatR;
using Reservation.Application.Commands;
using Reservation.Domain.Exceptions;
using Reservation.Domain.IRepositories;
using System.Threading;
using System.Threading.Tasks;

namespace Reservation.Application.CommandHandlers
{
    public class DeleteRoomReservationCommandHandler : IRequestHandler<DeleteRoomReservationCommand, bool>
    {
        private readonly IRoomReservationRepository _repository;

        public DeleteRoomReservationCommandHandler(IRoomReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteRoomReservationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ReservationException("request is null!");

            var roomReservation = await _repository.SingleOrDefaultAsync(x => x.Id == request.Id);
            await _repository.DeleteAsync(roomReservation);

            return true;
        }
    }
}