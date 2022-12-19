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
        private readonly ILocationRepository _locationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateRoomReservationCommandHandler(IRoomReservationRepository repository, 
                                                    ILocationRepository locationRepository, 
                                                    IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
            _locationRepository = locationRepository;
        }

        public async Task<bool> Handle(CreateRoomReservationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new RoomReservationException("request CreateRoomReservationCommand is null!");
            var location = await _locationRepository.FirstOrDefaultAsync(x => x.Id == request.LocationId);    
            var currentUserId = _contextAccessor.GetUserId();

            var period = Period.Create(request.StartDate, request.EndDate, location.Start, location.End);
            var roomReservation = new RoomReservation(currentUserId, request.RoomId, period);

            await _repository.CreateAsync(roomReservation);

            return true;
        }
    }
}