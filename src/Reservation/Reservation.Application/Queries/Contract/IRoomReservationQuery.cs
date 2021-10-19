﻿using Reservation.Domain.AggregatesModel;
using Reservation.Domain.ReadModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservation.Application.Queries.Contract
{
    public interface IRoomReservationQuery
    {
        Task<List<RoomReservationReadModel>> GetRoomReservations(Location location);
    }
}
