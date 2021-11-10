using MediatR;
using Microsoft.AspNetCore.Http;
using Reservation.Application.Commands;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.Exceptions;
using Reservation.Domain.Extensions;
using Reservation.Domain.IRepositories;
using System.Threading;
using System.Threading.Tasks;


namespace Reservation.Application.CommandHandlers
{
    public class CreateRoomReservationCommandHandler : IRequestHandler<CreateRoomReservationCommand, bool>
    {
        private readonly IRoomReservationRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateRoomReservationCommandHandler(IRoomReservationRepository repository, IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> Handle(CreateRoomReservationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ReservationException("request CreateRoomReservationCommand is null!");

            var currentUserId = _contextAccessor.GetUserId();

            var period = Period.Create(request.StartDate, request.EndDate);
            var roomReservation = new RoomReservation(request.RoomId, currentUserId, period);

            await _repository.CreateAsync(roomReservation);

            return true;
        }
    }
}