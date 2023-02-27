using MediatR;
using Microsoft.AspNetCore.Http;
using Reservation.Application.Commands;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.Exceptions;
using Reservation.Domain.Extensions;
using Reservation.Domain.IRepositories;
using Reservation.Domain.Utils;
using Reservation.Infrastructure.Context;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Reservation.Application.CommandHandlers
{
    public class CreateRoomReservationCommandHandler : IRequestHandler<CreateRoomReservationCommand, bool>
    {
        private readonly IRoomReservationRepository _repository;
        private readonly ILocationRepository _locationRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IClock _clock;
        private readonly ReservationDbContext _reservationDbContext;

        public CreateRoomReservationCommandHandler(IRoomReservationRepository repository,
                                                    ILocationRepository locationRepository,
                                                    IHttpContextAccessor contextAccessor,
                                                    IClock clock,
                                                    ReservationDbContext reservationDbContext)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
            _locationRepository = locationRepository;
            _clock = clock;
            _reservationDbContext = reservationDbContext;
        }

        public async Task<bool> Handle(CreateRoomReservationCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new RoomReservationException("request CreateRoomReservationCommand is null!");

            var location = await _locationRepository.FirstOrDefaultAsync(x => x.Id == request.LocationId);
            if (location == null)
                throw new RoomReservationException("location is null!");

            var currentUserId = _contextAccessor.GetUserId();

            var period = Period.Create(request.StartDate, request.EndDate, location.Start, location.End, _clock);
            var roomReservation = new RoomReservation(currentUserId, request.RoomId, period);

            using (var transaction = _reservationDbContext.Database.BeginTransaction())
            {
                await _reservationDbContext.AddAsync(roomReservation, cancellationToken);
                await _reservationDbContext.SaveChangesAsync(cancellationToken);

                transaction.Rollback();
            }
            using var tra = _reservationDbContext.Database.BeginTransactionAsync();
            //await _reservationDbContext.Database.BeginTransactionAsync();

            //await _repository.CreateAsync(roomReservation);


            //await tra.();
            return true;
        }
    }
}