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
    public class RoomReservationCommandHandler : IRequestHandler<RoomReservationCommand, bool>
    {
        private readonly IRoomReservationRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoomReservationCommandHandler(IRoomReservationRepository repository, IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> Handle(RoomReservationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ReservationException("request CreateRoomReservationCommand is null!");

            var userId = _contextAccessor.GetUser();

            var period = Period.Create(request.StartDate, request.EndDate, request.Location);
            var roomReservation = new RoomReservation(request.RoomId, userId, period, request.Location);

            await _repository.CreateAsync(roomReservation);

            return true;
        }
    }
}